using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Producer.KafkaProducer.Interface;
using Pay.Recorrencia.Gestao.Producer.Models;

namespace Pay.Recorrencia.Gestao.Infrastructure.Services
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private ILogger<KafkaProducerService> _logger;
        private readonly IProducer _producer;
        private InputParametersKafkaProducer _config;

        public KafkaProducerService(IOptions<InputParametersKafkaProducer> options, ILogger<KafkaProducerService> logger, IProducer producer)
        {
            _logger = logger;
            _producer = producer;
            _config = options.Value;
        }

        public async Task SendMessageAsync(string topic, string message)
        {

            try
            {
                var headers = new Headers();
                // Adicione cabeçalhos conforme necessário, por exemplo:
                headers.Add("key", new byte[] { 1, 2, 3 });

                await _producer.ProduceAsyncMessage(headers, message, _config.Producer.Topic);
                _logger.LogInformation($"Mensagem enviada");
            }
            catch (Exception e)
            {
                _logger.LogError($"Falha ao enviar mensagem: {e.Message}");
            }
        }
    }
}

