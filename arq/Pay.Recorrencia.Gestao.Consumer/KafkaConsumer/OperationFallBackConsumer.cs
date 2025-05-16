using Confluent.Kafka;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;

namespace Pay.Recorrencia.Gestao.Consumer.KafkaConsumer
{
    public class OperationFallBackConsumer : IConsumerOperation
    {
        public void Consume(string topic, int partition, string message, Headers headers, string topicWithEnvironment, long offSet)
        {
        }
    }
}
