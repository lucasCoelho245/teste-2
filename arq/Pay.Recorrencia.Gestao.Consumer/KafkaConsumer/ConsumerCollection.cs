using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;
using System.Collections.Concurrent;

namespace Pay.Recorrencia.Gestao.Consumer.KafkaConsumer
{
    public class ConsumerCollection
    {
        ConcurrentDictionary<string, IConsumerOperation> consumers = new();

        public void Add<T>(string key, T service) where T : IConsumerOperation
        {
            consumers[key] = service;
        }

        public void AddFallBack<T>(T service) where T : IConsumerOperation
        {
            consumers["FallBack"] = service;
        }

        public IConsumerOperation GetService(string key)
        {
            if (consumers.ContainsKey(key))
                return consumers[key];

            return null;
        }

        public IConsumerOperation GetServiceFallBack()
        {
            return consumers["FallBack"];
        }

        public int ServiceCount()
        {
            return consumers.Count;
        }
    }
}
