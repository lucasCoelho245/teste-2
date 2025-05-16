using Confluent.Kafka;
using Pay.Recorrencia.Gestao.Consumer.Models;
using System.Text;

namespace Pay.Recorrencia.Gestao.Consumer.KafkaConsumer
{
    public static class ConsumerExtensions
    {
        public static async ValueTask<ConsumeResult<TKey, TValue>> ConsumeAsync<TKey, TValue>(this IConsumer<TKey, TValue> consumer, CancellationToken ct)
        {
            IConsumer<TKey, TValue> consumer2 = consumer;
            try
            {
                return await Task.Run(delegate
                {
                    ConsumeResult<TKey, TValue> consumeResult = consumer2.Consume(ct);
                    GetConsumeResultMetadata(new ConsumeResultMetadata(consumeResult.Partition.Value, consumeResult.Offset.Value, consumeResult.Topic, consumeResult.Message.Headers.GetEntity()));
                    return consumeResult;
                }, ct);
            }
            catch (OperationCanceledException)
            {
                throw new TaskCanceledException("Kafka consumer [" + consumer2.Name + "] was canceled.");
            }
            catch (Exception ex2)
            {
                Exception e = ex2;
                throw new InvalidOperationException("ConsumeAsync [" + consumer2.Name + "] failed. Ex: " + e.Message + ".", e);
            }
        }

        public static ConsumeResultMetadata GetConsumeResultMetadata(ConsumeResultMetadata metadados)
        {
            return metadados;
        }

        public static List<TopicPartitionOffset> CommitConsumer<TKey, TValue>(this IConsumer<TKey, TValue> consumer)
        {
            return consumer.Commit();
        }

        public static string GetStringOrDefault(this Headers headers, string key)
        {
            byte[] lastHeader;
            return !headers.TryGetLastBytes(key, out lastHeader) ? "" : Encoding.ASCII.GetString(lastHeader);
        }

        public static string GetString(this Headers headers, string key)
        {
            return headers.GetStringOrDefault(key);
        }

        public static string GetEntity(this Headers headers)
        {
            return headers.GetStringOrDefault("ENTITY");
        }
    }
}
