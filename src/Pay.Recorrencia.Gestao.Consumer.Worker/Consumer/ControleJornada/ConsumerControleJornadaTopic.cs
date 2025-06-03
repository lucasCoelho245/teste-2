using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using Pay.Recorrencia.Gestao.Application.Commands.ControleJornada;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirInformacaoSolicitacao;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.ControleJornada.Validation;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Producer.Models;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.ControleJornada
{
    public class ConsumerControleJornadaTopic : IConsumerOperation
    {
        private readonly ILogger<ConsumerControleJornadaTopic> _logger;
        private readonly InputParametersKafkaProducer _inputParameterKafka;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IServiceScopeFactory _scopeFactory;
        private IMapper _mapper { get; }



        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public ConsumerControleJornadaTopic(ILogger<ConsumerControleJornadaTopic> logger,
                                            IOptions<InputParametersKafkaProducer> inputParameterKafka,
                                            IKafkaProducerService kafkaProducerService,
                                            IMapper mapper,
                                            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _inputParameterKafka = inputParameterKafka.Value;
            _kafkaProducerService = kafkaProducerService;
            _mapper = mapper;
            _scopeFactory = scopeFactory;

        }

        public async Task ConsumeAsync(string topic, int partition, string message, Headers headers, string topicWithEnvironment, long offSet)
        {

            ControleJornadaEntrada? dados = ConvertJsonToControleJornada(message);

            if (dados is null)
            {
                _logger.LogError("Erro: A mensagem JSON não pôde ser convertida para SolicitacaoRecorrencia.");
                return;
            }

            var validacao = CorrenteValidacao();
            var motivoRejeicao = validacao.Validar(dados);

            if (!string.IsNullOrEmpty(motivoRejeicao))
            {
                _logger.LogError($"Validação falhou: {motivoRejeicao}");
                return;
            }

            await BuscarControleAsync(dados);

        }

        private ValidarDadosEntrada CorrenteValidacao()
        {
            // Instanciação da cadeia de validação
            var validadorFinal = new ValidarFinal();
            var validadorDadosEntrada = new ValidarDadosEntrada(validadorFinal);

            return validadorDadosEntrada;
        }

        private async Task BuscarControleAsync(ControleJornadaEntrada dados)
        {
            _logger.LogInformation("🔍 Cenário 02: Consultando jornada existente..'.");

            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var request = new ListaControleJornadaAgendamentoAutorizacaoRequest
            {
                TpJornada = dados.TpJornada,
                IdRecorrencia = dados.IdRecorrencia,
                IdE2E = dados.IdE2E
            };

            var registros = await mediator.Send(request);

            switch (registros.Data.Items.Count())
            {
                case 0:
                    _logger.LogInformation("🟡 Nenhuma jornada encontrada. Ir para cenário 03");
                    await IncluirControleAsync(dados);
                    break;

                case 1:
                    _logger.LogInformation("🟢 Uma jornada encontrada. Ir para cenário 04");
                    var controleJornada = _mapper.Map<ControleJornadaEntrada>(registros.Data.Items.FirstOrDefault());    
                    await AtualizarControleAsync(controleJornada, dados);
                    break;

                default:
                    _logger.LogWarning("🔴 Mais de uma jornada encontrada. Ir para cenário 05");
                    await EnviarMensagemErroAsync(dados, "ERRO-PIXAUTO-022");
                    break;
            }
        }

        private async Task IncluirControleAsync(ControleJornadaEntrada dados)
        {
            _logger.LogInformation("🟡 Executando Cenário 03: Incluir novo registro de jornada...");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var inputControleJornada = new IncluirControleJornadaCommand
                {
                    IdE2E = dados.IdE2E ?? throw new ArgumentNullException(nameof(dados.IdE2E)),
                    IdRecorrencia = dados.IdRecorrencia,
                    TpJornada = dados.TpJornada,
                    SituacaoJornada = dados.SituacaoJornada,
                    DtAgendamento = dados.DtAgendamento,
                    DtPagamento = dados.DtPagamento,
                    DataUltimaAtualizacao = dados.DataUltimaAtualizacao
                };

                var registros = await mediator.Send(inputControleJornada);

                if (registros is null)
                {
                    _logger.LogWarning("⚠️ Inclusão do controle de jornada falhou. Seguir para cenário 05.");
                    await EnviarMensagemErroAsync(dados, "ERRO-PIXAUTO-021");
                    return;
                }

                _logger.LogInformation("✅ Inclusão do controle de jornada realizada com sucesso. Finalizar processo.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "🚨 Erro ao executar cenário 03 - inclusão de controle de jornada.");
                await EnviarMensagemErroAsync(dados, "ERRO-PIXAUTO-021");
            }
        }

        private async Task AtualizarControleAsync(ControleJornadaEntrada registroExistente, ControleJornadaEntrada novaEntrada)
        {
            _logger.LogInformation("🟢 Executando Cenário 04: Verificando atualização da jornada...");

            try
            {

                // Verificação de atualização
                if (novaEntrada.DataUltimaAtualizacao.HasValue &&
                    novaEntrada.DataUltimaAtualizacao <= registroExistente.DataUltimaAtualizacao)
                {
                    _logger.LogInformation("🟡 Dados recebidos são mais antigos. Registro não será atualizado.");
                    return; // Finaliza sem atualizar
                }

                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var updateControleJornada = new AtualizarControleJornadaCommand
                {
                    IdE2E = novaEntrada.IdE2E ?? throw new ArgumentNullException(nameof(novaEntrada.IdE2E)),
                    IdRecorrencia = novaEntrada.IdRecorrencia,
                    TpJornada = novaEntrada.TpJornada,
                    SituacaoJornada = novaEntrada.SituacaoJornada,
                    DtAgendamento = novaEntrada.DtAgendamento,
                    DtPagamento = novaEntrada.DtPagamento,
                    DataUltimaAtualizacao = novaEntrada.DataUltimaAtualizacao
                };

                var registros = await mediator.Send(updateControleJornada);

                if (registros is null)
                {
                    _logger.LogWarning("⚠️ Falha ao atualizar controle de jornada. Seguir para cenário 05.");
                    await EnviarMensagemErroAsync(registroExistente, "ERRO-PIXAUTO-020");
                    return;
                }

                _logger.LogInformation("✅ Atualização da jornada concluída com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "🚨 Erro ao executar cenário 04 - atualização de controle de jornada.");
                await EnviarMensagemErroAsync(registroExistente, "ERRO-PIXAUTO-020");
            }
        }

        private async Task EnviarMensagemErroAsync(ControleJornadaEntrada registros, string? codigoErro)
        {

            var message = _mapper.Map<MensagemControleJornada>(registros);
            message.CodigoDeErro = codigoErro;
            message.DtErro = DateTime.Now;

            string topicProducer = _inputParameterKafka?.Producer?.Topic ?? string.Empty;

            //transformar em json e enviar para uma fila
            var json = JsonConvert.SerializeObject(message, Formatting.Indented);

            //enviar mensagem
            await _kafkaProducerService.SendMessageAsync(topicProducer, json);

        }

        private ControleJornadaEntrada? ConvertJsonToControleJornada(string json)
        {
            try
            {
                json = Regex.Unescape(json);

                // Use the cached JsonSerializerOptions instance
                return JsonSerializer.Deserialize<ControleJornadaEntrada>(json, _jsonSerializerOptions); ;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Erro ao desserializar JSON: {ex.Message}");
                return null;
            }
        }
    }
}
