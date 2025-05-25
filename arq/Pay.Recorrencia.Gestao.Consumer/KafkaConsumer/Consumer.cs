using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Consumer.Models;

namespace Pay.Recorrencia.Gestao.Consumer.KafkaConsumer
{
    public class Consumer
    {
        private readonly ILogger<Consumer> _logger;
        private readonly InputParametersKafkaConsumer _inputParametersKafka;
        private readonly ConfigKafkaModel _configKafkaModel;

        public Consumer(IOptions<InputParametersKafkaConsumer> inputParametersKafka, ILogger<Consumer> logger)
        {
            _inputParametersKafka = inputParametersKafka.Value;
            _logger = logger;
            _configKafkaModel = new ConfigKafkaModel();
        }

        public IConsumer<Null, string> GetKafkaConsumer()
        {
            _logger.LogInformation("Creating Kafka consumer with default parameters.");
            return GetKafkaConsumer(GetTopics(), _inputParametersKafka?.Consumer?.GroupId, _inputParametersKafka.Consumer.EnableAutoCommit);
        }

        public IConsumer<Null, string> GetKafkaConsumer(int startPartition, int endPartition)
        {
            _logger.LogInformation("Creating Kafka consumer for partitions [{StartPartition} - {EndPartition}] on topic {Topic}.", startPartition, endPartition, GetTopics());
            return GetKafkaConsumerByPartition(GetTopics(), startPartition, endPartition);
        }

        public IConsumer<Null, string> GetKafkaConsumer(int[] partitions)
        {
            _logger.LogInformation("Creating Kafka consumer for partitions [{Partitions}] on topic {Topic}.", string.Join(", ", partitions), GetTopics());
            return GetKafkaConsumerByPartition(GetTopics(), partitions);
        }

        public IConsumer<Null, string> GetKafkaConsumer(string topicName)
        {
            _logger.LogInformation("Creating Kafka consumer for topic {TopicName} with default group ID and auto commit settings.", topicName);
            return GetKafkaConsumer(new[] { topicName }, _inputParametersKafka.Consumer.GroupId, _inputParametersKafka.Consumer.EnableAutoCommit);
        }

        public IConsumer<Null, string> GetKafkaConsumer(string topicName, string groupId)
        {
            _logger.LogInformation("Creating Kafka consumer for topic {TopicName} with group ID {GroupId} and default auto commit settings.", topicName, groupId);
            return GetKafkaConsumer(new[] { topicName }, groupId, _inputParametersKafka.Consumer.EnableAutoCommit);
        }

        public IConsumer<Null, string> GetKafkaConsumer(string[] topicNames, string groupId, bool enableAutoCommit)
        {
            _logger.LogInformation("Creating Kafka consumer for topics [{TopicNames}] with group ID {GroupId} and auto commit set to {EnableAutoCommit}.", string.Join(", ", topicNames), groupId, enableAutoCommit);
            foreach (var topic in topicNames)
            {
                if (!TopicExists(topic))
                {
                    throw new Exception($"Topic {topic} does not exist.");
                }
            }
            var consumer = GetConsumerBuilder();
            _logger.LogInformation("Subscribing to topics: {Topics}", string.Join(", ", topicNames));
            consumer.Subscribe(topicNames);
            return consumer;
        }

        public IConsumer<Null, string> GetKafkaConsumerByPartition(string[] topicNames, int startPartition, int endPartition)
        {
            _logger.LogInformation("Creating Kafka consumer for topics [{TopicNames}] and assigning partitions [{StartPartition} - {EndPartition}].",
                string.Join(", ", topicNames), startPartition, endPartition);

            var consumer = GetConsumerBuilder();
            var partitions = new List<TopicPartition>();
            foreach (var topic in topicNames)
            {
                partitions.AddRange(AssignPartitions(topic, startPartition, endPartition));
            }
            consumer.Assign(partitions);

            _logger.LogInformation("Consumer assigned to partitions [{StartPartition} - {EndPartition}] on topics [{TopicNames}]",
                startPartition, endPartition, string.Join(", ", topicNames));
            return consumer;
        }

        public IConsumer<Null, string> GetKafkaConsumerByPartition(string[] topicNames, int[] partitions)
        {
            _logger.LogInformation("Creating Kafka consumer for topics [{TopicNames}] and assigning partitions [{Partitions}].",
                string.Join(", ", topicNames), string.Join(", ", partitions));

            var consumer = GetConsumerBuilder();
            var assignedPartitions = new List<TopicPartition>();
            foreach (var topic in topicNames)
            {
                assignedPartitions.AddRange(AssignPartitions(topic, partitions));
            }
            consumer.Assign(assignedPartitions);

            _logger.LogInformation("Consumer assigned to partitions [{Partitions}] on topics [{TopicNames}]",
                string.Join(", ", partitions), string.Join(", ", topicNames));
            return consumer;
        }


        public void StartConsumerLoop()
        {
            var consumer = GetKafkaConsumer();
            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume(CancellationToken.None);
                    if (consumeResult?.Message != null)
                    {
                        _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error consuming message: {ex.Message}");
            }
            finally
            {
                consumer.Close();
            }
        }

        #region private methods

        private IConsumer<Null, string> GetConsumerBuilder()
        {
            var _config = ConfigKafkaConsumerWithScramSha512();

            var consumerBuilder = new ConsumerBuilder<Null, string>(_config);

            return consumerBuilder.SetLogHandler((_, error) =>
            {
                _logger.Log(LogLevel.Information, $"ConsumerBuilder: {error}");
                CheckErrorConsumer(error);
            })
            .SetErrorHandler((_, error) =>
            {
                _logger.Log(LogLevel.Error, $"ConsumerBuilder: {error}");
                CheckErrorConsumer(error);
            })
            .SetPartitionsAssignedHandler((c, partitions) =>
            {
                if (partitions.Count > 0)
                {
                    _logger.Log(LogLevel.Information, $"Partitions assigned: [{string.Join(", ", partitions)}]");
                }
                else
                {
                    _logger.LogWarning("No partitions assigned.");
                }
            })
            .SetPartitionsRevokedHandler((c, partitions) => _logger.LogWarning($"Partitions revoked: [{string.Join(", ", partitions)}]"))
            .Build();
        }

        private void CheckErrorConsumer(Error error)
        {
            if (error.IsFatal || error.Code == ErrorCode.Local_MaxPollExceeded)
            {
                _logger.LogError($"Fatal Kafka error: {error.Reason}. Restarting consumer...");
                RestartConsumer();
            }
        }

        private void CheckErrorConsumer(LogMessage error)
        {
            if (error.Message?.Contains("leaving group", StringComparison.OrdinalIgnoreCase) == true)
            {
                _logger.LogError($"Consumer left group: {error.Message}. Restarting consumer...");
                RestartConsumer();
            }
        }

        private void RestartConsumer()
        {
            _logger.LogInformation("Restarting Kafka consumer...");
            StartConsumerLoop();
        }

        private bool TopicExists(string topicName)
        {
            using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _inputParametersKafka.BootstrapServers }).Build();
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(3));
            return metadata.Topics.Any(t => t.Topic == topicName);
        }

        private string[] GetTopics()
        {
            return _inputParametersKafka?.Consumer?.KafkaConsumerMappings?
                        .Select(m => m.Topic)
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .ToArray() ?? [];
        }

        private ConsumerConfig ConfigKafkaConsumerWithScramSha512()
        {
            var saslMechanism = _inputParametersKafka.SecurityParameters?.ActivateSsl == true ? SaslMechanism.ScramSha512 : (SaslMechanism?)null;
            var securityProtocol = _inputParametersKafka.SecurityParameters?.ActivateSsl == true ? SecurityProtocol.SaslSsl : SecurityProtocol.Plaintext;

            return _configKafkaModel.ConsumerConfigInit(
                _inputParametersKafka.BootstrapServers,
                saslMechanism,
                securityProtocol,
                _inputParametersKafka.SecurityParameters?.CertifiedPath ?? string.Empty,
                _inputParametersKafka.SecurityParameters?.SaslUsername ?? string.Empty,
                _inputParametersKafka.SecurityParameters?.SaslPassword ?? string.Empty,
                _inputParametersKafka.MessageMaxBytes,
                _inputParametersKafka.Consumer.ConsumerProccessMaxMS,
                true, // Always read from the beginning if no offset is saved
                _inputParametersKafka.Consumer.GroupId,
                _inputParametersKafka.Consumer.HeartbeatIntervalMs ?? 3000, // Default to 3 seconds if not set
                _inputParametersKafka.Consumer.SessionTimeoutMs ?? 10000, // Default to 10 seconds if not set
                _inputParametersKafka.Consumer.MaxPollIntervalMs ?? 300000, // Default to 5 minutes if not set
                _inputParametersKafka.Consumer.Debug
            );
        }

        private static List<string> TransformStringIntoConsumerTopicLists(string consumedTopicsNames)
        {
            if (string.IsNullOrWhiteSpace(consumedTopicsNames)) throw new Exception("TopicsConsumed was not defined in AppSettings.json");
            return consumedTopicsNames.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        private static List<TopicPartition> AssignPartitions(string topicName, int[] partitions)
        {
            return partitions.Select(p => new TopicPartition(topicName, new Partition(p))).ToList();
        }

        private static List<TopicPartition> AssignPartitions(string topicName, int startPartition, int endPartition)
        {
            var partitions = new List<TopicPartition>();
            for (int partition = startPartition; partition <= endPartition; partition++)
            {
                partitions.Add(new TopicPartition(topicName, new Partition(partition)));
            }
            return partitions;
        }

        #endregion
    }
}