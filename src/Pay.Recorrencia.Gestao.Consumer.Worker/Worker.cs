using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer;
using Pay.Recorrencia.Gestao.Consumer.Models;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer;

namespace Pay.Recorrencia.Gestao.Consumer.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IServiceProvider _serviceProvider;
        private readonly InputParametersKafkaConsumer _kafkaSettings;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IOptions<InputParametersKafkaConsumer> kafkaSettings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _kafkaSettings = kafkaSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                string ConsumedTopics = _kafkaSettings?.Consumer?.ConsumedTopics ?? string.Empty;

                if (string.IsNullOrEmpty(ConsumedTopics))
                    _logger.LogWarning($"Parâmetro de tópico não preenchido.");
                else
                {
                    _logger.LogInformation($"Consumer iniciado - {DateTime.Now.ToShortTimeString()}.");
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var consumidorServicesOptions = new ConsumerServicesMapper(scope.ServiceProvider)
                            .MapToFallback<OperationFallBackConsumer>()
                            .MapToTopicTransaction<ConsumerCustomTopic>(ConsumedTopics);

                        await Task.Delay(1000, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro na execução do Worker: {ex.Message}");
            }
        }
    }
}
