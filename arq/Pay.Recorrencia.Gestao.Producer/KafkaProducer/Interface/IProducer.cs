using Confluent.Kafka;

namespace Pay.Recorrencia.Gestao.Producer.KafkaProducer.Interface
{
    public interface IProducer
    {
        public Task ProduceAsyncMessage(Headers headers, string serializedModel, string? topic);
    }
}
