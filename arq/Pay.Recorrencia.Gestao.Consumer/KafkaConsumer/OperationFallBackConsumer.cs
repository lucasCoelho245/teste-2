using Confluent.Kafka;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;

namespace Pay.Recorrencia.Gestao.Consumer.KafkaConsumer
{
    public class OperationFallBackConsumer : IConsumerOperation
    {
        public Task ConsumeAsync(string topic, int partition, string message, Headers headers, string topicWithEnvironment, long offSet)
        {
            // Add logic here to handle the message consumption.
            // For now, returning a completed task to ensure all code paths return a value.
            return Task.CompletedTask;
        }
    }
}
