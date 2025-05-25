using Confluent.Kafka;

namespace Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface
{
    public interface IConsumerOperation
    {
        Task ConsumeAsync(string topic, int partition, string message, Headers headers, string topicWithEnviroment, long offSet);
    }
}
