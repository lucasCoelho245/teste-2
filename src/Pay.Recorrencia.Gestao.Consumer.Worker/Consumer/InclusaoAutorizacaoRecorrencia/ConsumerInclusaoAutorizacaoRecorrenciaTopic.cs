using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoSequencial;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;
using Pay.Recorrencia.Gestao.Consumer.Models;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.InclusaoAutorizacaoRecorrencia.Validation;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Services;
using System.Text;
using System.Text.Json;

namespace Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.InclusaoAutorizacaoRecorrencia
{
    public class ConsumerInclusaoAutorizacaoRecorrenciaTopic : IConsumerOperation
    {
        private readonly ILogger<ConsumerInclusaoAutorizacaoRecorrenciaTopic> _logger;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly InputParametersKafkaConsumer _kafkaSettings;
        private readonly IServiceScopeFactory _scopeFactory;

        public ConsumerInclusaoAutorizacaoRecorrenciaTopic(
            ILogger<ConsumerInclusaoAutorizacaoRecorrenciaTopic> logger, 
            IKafkaProducerService kafkaProducerService, 
            IOptions<InputParametersKafkaConsumer> kafkaSettings,
            IServiceScopeFactory scopeFactory
            )
        {
            _logger = logger;
            _kafkaProducerService = kafkaProducerService;
            _kafkaSettings = kafkaSettings.Value;
            _scopeFactory = scopeFactory;
        }

        public async Task ConsumeAsync(string topic, int partition, string message, Headers headers, string topicWithEnvironment, long offSet)
        {
            // Converte JSON
            SolicitacaoAutorizacaoRecorrencia? dadosSolicitacao = ConvertJsonToSolicitacaoAutorizacaoRecorrencia(message);
            if (dadosSolicitacao == null)
            {
                _logger.LogError("Erro: A mensagem JSON não pôde ser convertida para SolicitacaoAutorizacaoRecorrencia.");
                return;
            }

            // Valida os campos
            var isValid = ValidateObjectDadosSolicitacao(dadosSolicitacao);
            if (!isValid) { return; }

            // Gera o novo ID de autorização
            var idAutorizacao = await GeraIdentificadorDeAutorizacao();

            // Cria novo objeto com o ID e chama endpoint para salvar no banco
            await ArmazenaDadosDaSolicitacao(dadosSolicitacao, idAutorizacao);

            // Envia novo evento
            string topicProducer = _kafkaSettings?.Producer?.TopicInclusaoAutorizacaoRecorrencia ?? string.Empty;
            var responseMessage = new RecorrenciaIncluidaDTO(dadosSolicitacao);

            RelayEventHubMessage(topicProducer, JsonSerializer.Serialize(responseMessage));
        }

        public SolicitacaoAutorizacaoRecorrencia? ConvertJsonToSolicitacaoAutorizacaoRecorrencia(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<SolicitacaoAutorizacaoRecorrencia>(json, options);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Erro ao desserializar JSON: {ex.Message}");
                return null;
            }
        }

        public async Task ArmazenaDadosDaSolicitacao(SolicitacaoAutorizacaoRecorrencia dadosSolicitacao, string idAutorizacao)
        {
            try
            {
                var request = new InserirAutorizacaoRecorrenciaCommand(dadosSolicitacao, idAutorizacao);

                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Armazenamento dos dados da solicitação falhou: {ex.Message}");
                throw;
            }
        }


        public bool ValidateObjectDadosSolicitacao(SolicitacaoAutorizacaoRecorrencia OSolAuthRec)
        {
            var isValid = SolicitacaoAutorizacaoRecorrenciaValidator.Validar(OSolAuthRec);

            if (!isValid)
            {
                _logger.LogError("Erro: Campos não preenchidos corretamente.");
                Console.WriteLine("Campos não preenchidos corretamente");
                return false;
            }

            return true;
        }

        public async void RelayEventHubMessage(string topic, string message)
        {
            await _kafkaProducerService.SendMessageWithParameterAsync(topic, message);
        }

        public async Task<string> GeraIdentificadorDeAutorizacao()
        {
            try
            {
                var request = new SolicitacaoSequencialCommand();

                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                long numeroAtual = await mediator.Send(request);

                string sequencial = GerarSequenciaAlfanumerico(11, numeroAtual);
                string dataCriacaoAutorizacao = DateTime.UtcNow.ToString("yyyyMMdd");

                return "AR" + dataCriacaoAutorizacao + sequencial;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Criação de identificador de autorização falhou: {ex.Message}");
                throw;
            }
        }

        public static string GerarSequenciaAlfanumerico(int tamanho, long numeroAtual)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            if (numeroAtual == 0)
            {
                return numeroAtual.ToString().PadLeft(tamanho, '0');
            }

            var result = new StringBuilder(tamanho);
            while (numeroAtual > 0)
            {
                result.Insert(0, chars[(int)(numeroAtual % 62)]);
                numeroAtual /= 62;
            }

            return result.ToString().PadLeft(tamanho, '0'); ;
        }
    }
}
