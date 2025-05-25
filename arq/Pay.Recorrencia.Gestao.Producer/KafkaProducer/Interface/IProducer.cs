using Confluent.Kafka;

namespace Pay.Recorrencia.Gestao.Producer.KafkaProducer.Interface
{
    public interface IProducer
    {
        public Task ProduceAsyncMessage(Headers headers, string serializedModel, string? topic);

        public Task EnviarEventoAsync<T>(T evento, string? topic) where T : class;

    }
}
