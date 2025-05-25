using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirInformacaoSolicitacao;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation.IValidation;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Events;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Producer.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado
{
    public class ConsumerPayPagamentoProcessadoTopic : IConsumerOperation
    {
        //private IMediator _mediator { get; }
        private readonly ILogger<ConsumerPayPagamentoProcessadoTopic> _logger;
        private readonly InputParametersKafkaProducer _inputParameterKafka;
        private readonly IKafkaProducerService _kafkaProducerService;
        private IMapper _mapper { get; }
        //private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public ConsumerPayPagamentoProcessadoTopic(ILogger<ConsumerPayPagamentoProcessadoTopic> logger
                                                 , IMapper mapper
                                                 , IOptions<InputParametersKafkaProducer> inputParameterKafka
                                                 , IKafkaProducerService kafkaProducerService
                                                 , IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _mapper = mapper;
            _inputParameterKafka = inputParameterKafka.Value;
            _kafkaProducerService = kafkaProducerService;
            _scopeFactory = scopeFactory;
        }

        public async Task ConsumeAsync(string topic, int partition, string message, Headers headers, string topicWithEnvironment, long offSet)
        {
            string motivoRejeicao = string.Empty;

            //Converter mensagem JSON para objeto SolicitacaoRecorrencia
            SolicitacaoRecorrenciaEntrada? dados = ConvertJsonToSolicitacaoRecorrencia(message);

            if (dados == null)
            {
                _logger.LogError("Erro: A mensagem JSON não pôde ser convertida para SolicitacaoRecorrencia.");
                return;
            }

            //Instanciar objeto para validar os dados (Chain of responsibility pattern)
            var cadeiaValidacao = InstanciarCorrenteValidacao();

            // Validar dados de entrada, dados da conta e dados de recorrência
            motivoRejeicao = cadeiaValidacao.Validar(dados);

            // Gerar ID de informação de status
            var idInformacaoStatus = await GerarIdInformacaoStatusAsync(dados.ParticipanteDoUsuarioRecebedor);

            if (string.IsNullOrEmpty(motivoRejeicao))
                // Incluir solicitação de autorização de recorrência em bases internas
                await IncluirSolicitacaoAutorizacaoRecorrencia(dados);

            // Adicionar informação em fila para a geração do PAIN.012 de retorno (sucesso ou falha)
            EnviarRespostaSolicitacaoRecorrencia(dados, motivoRejeicao);
        }

        private IValidacaoSolicitacao InstanciarCorrenteValidacao()
        {
            using var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IValidacaoSolicitacao>();
        }

        /// <summary>
        /// Geração do idInformacaoStatus
        /// </summary>
        /// <param name="dados"></param>
        private async Task<string> GerarIdInformacaoStatusAsync(string ispb)
        {
            long sequencial = default;
            try
            {
                ispb = (ispb ?? string.Empty).PadLeft(8, '0').Substring(0, 8);
                var data = DateTime.UtcNow.ToString("yyyyMMdd");

                using (var scope = _scopeFactory.CreateScope())
                {
                    IncluirInformacaoSolicitacaoRequest request = new(ispb);
                    // Cria um escopo para resolver o IMediator
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    sequencial = await mediator.Send(request);

                }

                string sequencialAlfanumerico = sequencial.ToString("D11");
                return $"IS{ispb}{data}{sequencialAlfanumerico}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao gerar ID de informação de status: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Incluir  solicitação de autorização de recorrência em bases internas
        /// </summary>
        /// <param name="dados"></param>
        private async Task IncluirSolicitacaoAutorizacaoRecorrencia(SolicitacaoRecorrenciaEntrada dados)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var command = _mapper.Map<IncluirSolicitacaoRecorrenciaCommand>(dados);

                // Envie o comando para o handler
                var resultado = await mediator.Send(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao incluir solicitação de autorização de recorrência.");
                throw;
            }
        }

        #region métodos privados
        private SolicitacaoRecorrenciaEntrada? ConvertJsonToSolicitacaoRecorrencia(string json)
        {
            try
            {
                json = Regex.Unescape(json);

                // Use the cached JsonSerializerOptions instance
                var solicitacaoRecorrencia = JsonSerializer.Deserialize<SolicitacaoRecorrenciaEntrada>(json, _jsonSerializerOptions);

                return solicitacaoRecorrencia;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Erro ao desserializar JSON: {ex.Message}");
                return null;
            }
        }


        private void EmitirEventoControleJornada(SolicitacaoRecorrenciaEntrada dados)
        {
            try
            {
                var evento = new ControleJornadaEvent
                {
                    TpJornada = "Jornada 1",
                    IdRecorrencia = dados.IdRecorrencia,
                    SituacaoJornada = "Solicitação Recebida",
                    DataUltimaAtualizacao = DateTime.UtcNow.ToString("o") // ISO 8601
                };

                var json = JsonConvert.SerializeObject(evento, Formatting.Indented);

                // Tópico configurado no appsettings (Producer > TopicControleJornada)
                string topic = _inputParameterKafka.TopicControleJornada;

                _kafkaProducerService.SendMessageAsync(topic, json);

                _logger.LogInformation("📤 Evento de controle de jornada enviado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Erro ao emitir evento de controle de jornada: {ex.Message}");
            }
        }

        //todo: esse método é necessario?
        private async Task AtualizarControleDePedidos()
        {
            var evento = new AtualizarControleDePedidos
            {
                tpJornada = "",
                idRecorrecina = "",
                situacaoJornada = "",
                dataUltimaAtualizacao = ""
            };

            using var scope = _scopeFactory.CreateScope();
            var kafkaProducer = scope.ServiceProvider.GetRequiredService<IKafkaProducerService>();

            //await kafkaProducer.EnviarEventoAsync(evento, _kafkaSettings.Producer.Topic);

            _logger.LogInformation($"Evento AtualizarControleDePedidos enviado com sucesso.");
        }

        private static async Task EnviarPayloadPain012Async(Dictionary<string, object> payload)
        {
            using var httpClient = new HttpClient();
            var url = "https://proxy.spb.gov.br/api/Entrada/AutorizarRecorrencia";

            try
            {
                var response = await httpClient.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("✅ Payload PAIN.012 enviado com sucesso!");
                }
                else
                {
                    Console.WriteLine($"❌ Falha ao enviar PAIN.012: {response.StatusCode}");
                    var respostaErro = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"🔍 Detalhes: {respostaErro}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"🚨 Erro ao enviar PAIN.012: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Erro inesperado: {ex.Message}");
            }
        }

        /// <summary>
        ///  Aceitar/rejeitar recebimento de solicitação de recorrência
        /// </summary>
        /// <param name="dados"></param>
        /// <param name="motivoRejeicao"></param>
        private void EnviarRespostaSolicitacaoRecorrencia(SolicitacaoRecorrenciaEntrada dados, string? motivoRejeicao)
        {
            try
            {
                var message = MapToProcessamentoRecorrencia(dados, motivoRejeicao);
                string topicProducer = _inputParameterKafka?.Producer?.Topic ?? string.Empty;
                var json = JsonConvert.SerializeObject(message, Formatting.Indented);
                _kafkaProducerService.SendMessageAsync(topicProducer, json);
                _logger.LogInformation("A resposta da recorrência foi enviada.");
            }
            catch (Exception e)
            {
                _logger.LogError("Erro: A resposta da recorrência não pôde ser enviada.");
                throw;
            }
        }

        /// <summary>
        /// Mapear SolicitacaoRecorrenciaEntrada para ProcessamentoRecorrencia
        /// </summary>
        /// <param name="dados"></param>
        /// <param name="motivoRejeicao"></param>
        private ProcessamentoRecorrencia MapToProcessamentoRecorrencia(SolicitacaoRecorrenciaEntrada dados, string? motivoRejeicao)
        {
            try
            {
                var message = _mapper.Map<ProcessamentoRecorrencia>(dados);
                message.MotivoRejeicao = motivoRejeicao;
                message.Status = string.IsNullOrEmpty(message.MotivoRejeicao);
                message.SituacaoRecorrencia = message.Status ? "PDNG" : string.Empty;
                message.DataUltimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                return message;
            }
            catch (Exception e)
            {
                _logger.LogError("Erro: Não foi possível mapear os dados SolicitacaoRecorrenciaEntrada para ProcessamentoRecorrencia.");
                throw;
            }
        }

        #endregion

    }
}
