using Confluent.Kafka;
using Pay.Recorrencia.Gestao.Producer.KafkaProducer.Interface;
using Serilog;

namespace Pay.Recorrencia.Gestao.Producer.KafkaProducer
{
    public class FakeProducer<B, A> : IProducer<B, A>
    {
        public Handle Handle
        {
            get
            {
                Log.Information("Kafka está desabilitado.");
                return new Handle { };
            }
        }

        public string Name => "";

        public void AbortTransaction(TimeSpan timeout)
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public void AbortTransaction()
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public int AddBrokers(string brokers)
        {
            Log.Information($"Kafka está desabilitado.");
            return 0;
        }

        public void BeginTransaction()
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public void CommitTransaction(TimeSpan timeout)
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public void CommitTransaction()
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public void Dispose()
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public int Flush(TimeSpan timeout)
        {
            Log.Information($"Kafka está desabilitado.");
            return 0;
        }

        public void Flush(CancellationToken cancellationToken = default)
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public void InitTransactions(TimeSpan timeout)
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public int Poll(TimeSpan timeout)
        {
            Log.Information($"Kafka está desabilitado.");
            return 0;
        }

        public void Produce(string topic, Message<B, A> message, Action<DeliveryReport<B, A>> deliveryHandler = null)
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public void Produce(TopicPartition topicPartition, Message<B, A> message, Action<DeliveryReport<B, A>> deliveryHandler = null)
        {
            Log.Information($"Kafka está desabilitado.");
        }

        public Task<DeliveryResult<B, A>> ProduceAsync(string topic, Message<B, A> message, CancellationToken cancellationToken = default)
        {
            Log.Information("Kafka está desabilitado.");
            var teste = new DeliveryResult<B, A>();
            return Task.FromResult(teste);

        }

        public Task<DeliveryResult<B, A>> ProduceAsync(TopicPartition topicPartition, Message<B, A> message, CancellationToken cancellationToken = default)
        {
            Log.Information("Kafka está desabilitado.");
            var teste = new DeliveryResult<B, A>();
            return Task.FromResult(teste);

        }

        public void SendOffsetsToTransaction(IEnumerable<TopicPartitionOffset> offsets, IConsumerGroupMetadata groupMetadata, TimeSpan timeout)
        {
            Log.Information("Kafka está desabilitado.");
        }

        public void SetSaslCredentials(string username, string password)
        {
            Log.Information("Kafka está desabilitado.");
        }

    }
    public class ProducerDisabled : IProducer
    {
        public ProducerDisabled()
        {
            Log.Information("Kafka está desabilitado.");
        }

        public IProducer<Null, string> GetKafkaProducer()
        {
            Log.Information("Kafka está desabilitado.");
            return new FakeProducer<Null, string>();
        }

        public async Task ProduceAsyncMessage(Headers headers, string serializedModel, string? topico)
        {
            Log.Information("Kafka está desabilitado.");
        }

        public async Task EnviarEventoAsync<T>(T evento, string? topic) where T : class
        {
            Log.Information("Kafka está desabilitado.");
        }
    }
}
