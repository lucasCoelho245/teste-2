using Microsoft.Extensions.DependencyInjection;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;

namespace Pay.Recorrencia.Gestao.Consumer.KafkaConsumer
{
    public class ConsumerServicesMapper
    {
        private readonly IServiceProvider provider;
        private readonly ConsumerCollection consumersCollection;

        public ConsumerServicesMapper(IServiceProvider provider)
        {
            this.provider = provider;
            consumersCollection = consumersCollection = provider.GetRequiredService<ConsumerCollection>();
        }

        public ConsumerServicesMapper MapToFallback<T>() where T : IConsumerOperation
        {
            using (var scope = provider.CreateScope())
            {
                T service = scope.ServiceProvider.GetService<T>();
                consumersCollection.AddFallBack(service);
            }

            return this;
        }

        public ConsumerServicesMapper MapToTopicTransaction<T>(string topic) where T : IConsumerOperation
        {
            using (var scope = provider.CreateScope())
            {
                T service = scope.ServiceProvider.GetService<T>();
                consumersCollection.Add(topic, service);
            }

            return this;
        }
    }
}
