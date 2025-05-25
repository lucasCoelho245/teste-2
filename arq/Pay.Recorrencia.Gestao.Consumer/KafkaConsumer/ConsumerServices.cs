using Confluent.Kafka;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;

namespace Pay.Recorrencia.Gestao.Consumer.KafkaConsumer
{
    public class ConsumerServices
    {
        private readonly ConsumerCollection consumersCollection;

        public ConsumerServices(ConsumerCollection consumersCollection)
        {
            this.consumersCollection = consumersCollection;
        }

        public void Consume(ConsumeResult<Null, string> messageConsumed, string topic)
        {
            string message = messageConsumed.Message.Value;
            int partition = messageConsumed.Partition.Value;
            long offSet = messageConsumed.Offset.Value;

            IConsumerOperation consumerOperation = GetOperation(topic);

            consumerOperation.ConsumeAsync(
                topic: topic,
                partition: partition,
                message: message,
                headers: messageConsumed.Message.Headers,
                topicWithEnviroment: messageConsumed.Topic,
                offSet: offSet
            );
        }

        private IConsumerOperation GetOperation(string topic)
        {
            string chaveTopico = GetKeyTopic(topic);
            IConsumerOperation operationConsumer;

            while (true)
            {
                int count = consumersCollection.ServiceCount();
                if (count == 0)
                {
                    Task.Delay(1_000);
                }
                else
                {
                    break;
                }
            }

            operationConsumer = consumersCollection.GetService(chaveTopico);

            if (operationConsumer == null)
            {
                operationConsumer = consumersCollection.GetServiceFallBack();
            }

            return operationConsumer;
        }

        public static string GetKeyTopic(string topic)
        {
            return $"{topic}";
        }
    }
}
