using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Domain.Settings;

namespace Pay.Recorrencia.Gestao.Application.Query.Notifications.KafkaConsumer
{
    public class KafkaConsumerHandler : INotificationHandler<KafkaConsumerNotification>
    {

        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly ILogger<KafkaConsumerHandler> _logger;
        private KafkaSettings _settings;

        public KafkaConsumerHandler(IKafkaProducerService kafkaProducerService, ILogger<KafkaConsumerHandler> logger, IOptions<KafkaSettings> options)
        {
            _kafkaProducerService = kafkaProducerService;
            _logger = logger;
            _settings = options.Value;
        }

        public async Task Handle(KafkaConsumerNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Tópico: {notification.Topic} - Mensagem: {notification.Message}");


        }
    }
}
