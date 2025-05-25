using Confluent.Kafka;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;

namespace Pay.Recorrencia.Gestao.Api.Consumidores
{
    public class TopicCustom : IConsumerOperation
    {
        private readonly ILogger<TopicCustom> _logger;

        public TopicCustom(ILogger<TopicCustom> logger)
        {
            _logger = logger;
        }

        public async Task ConsumeAsync(string topic, int partition, string message, Headers headers, string topicWithEnviroment, long offSet)
        {
            try
            {
                // Regras de Negocio que devem ser feitas quando consumir a mensagem.....
                _logger.LogInformation($"Consuming message from topic: {topic}, partition: {partition}, offset: {offSet}");
                // Add your business logic here
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message.");
            }

            // Ensure the method always returns a Task
            await Task.CompletedTask;
        }
    }
}
