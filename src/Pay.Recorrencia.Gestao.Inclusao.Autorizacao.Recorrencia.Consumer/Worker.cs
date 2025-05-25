using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;
using Pay.Recorrencia.Gestao.Consumer.Models;
using Pay.Recorrencia.Gestao.Inclusao.Autorizacao.Recorrencia.Consumer.Consumer;
using System.Reflection;

namespace Pay.Recorrencia.Gestao.Inclusao.Autorizacao.Recorrencia.Consumer
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
                var _consumerMappings = _kafkaSettings?.Consumer?.KafkaConsumerMappings;

                if (_consumerMappings == null || _consumerMappings.Count == 0)
                {
                    _logger.LogWarning("Nenhum mapeamento de consumidor configurado.");
                    return;
                }

                _logger.LogInformation("Consumer iniciado - {Time}.", DateTime.Now.ToShortTimeString());

                using var scope = _serviceProvider.CreateScope();
                var mapper = new ConsumerServicesMapper(scope.ServiceProvider).MapToFallback<OperationFallBackConsumer>();

                foreach (var mapping in _consumerMappings)
                {
                    // Procura o tipo pelo nome (namespace completo é recomendado)
                    var consumerType = Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .FirstOrDefault(t => t.Name == mapping.ConsumerType && typeof(IConsumerOperation).IsAssignableFrom(t));

                    if (consumerType == null)
                    {
                        _logger.LogWarning("Tipo de consumidor não encontrado: {ConsumerType}", mapping.ConsumerType);
                        continue;
                    }

                    // Usa reflexão para chamar MapToTopicTransaction<T>
                    var method = typeof(ConsumerServicesMapper).GetMethod("MapToTopicTransaction")?.MakeGenericMethod(consumerType);
                    method?.Invoke(mapper, [mapping.Topic]);
                }

                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na execução do Worker: {ErrorMessage}", ex.Message);
            }
        }
    }
}
