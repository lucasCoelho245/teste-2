using Confluent.Kafka;

namespace Pay.Recorrencia.Gestao.Consumer.Models
{
    public class ConfigKafkaModel
    {
        public ConfigKafkaModel() { }

        public ConsumerConfig ConsumerConfigInit(
            string bootstrapServers,
            SaslMechanism? saslMechanism,
            SecurityProtocol securityProtocol,
            string sslCaLocation,
            string saslUsername,
            string saslPassword,
            int? messageMaxBytes,
            int socketTimeoutMs,
            bool autoOffsetReset,
            string groupId,
            int? heartbeatIntervalMs,
            int? sessionTimeoutMs,
            int? maxPollIntervalMs,
            string? debug
            )
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                SecurityProtocol = securityProtocol,
                SslCaLocation = string.IsNullOrEmpty(sslCaLocation) ? @"/app/ca.crt" : sslCaLocation,
                SaslUsername = saslUsername,
                SaslPassword = saslPassword,
                MessageMaxBytes = messageMaxBytes ?? 1000000,
                SocketTimeoutMs = socketTimeoutMs,
                AutoOffsetReset = autoOffsetReset ? AutoOffsetReset.Earliest : AutoOffsetReset.Latest,
                GroupId = groupId,
                EnableAutoCommit = false,
                HeartbeatIntervalMs = heartbeatIntervalMs ?? 3000,
                SessionTimeoutMs = sessionTimeoutMs ?? 45000,
                MaxPollIntervalMs = maxPollIntervalMs ?? 300000,
                Debug = debug
            };

            if (saslMechanism.HasValue)
            {
                config.SaslMechanism = saslMechanism.Value;
            }

            return config;
        }
    }
}
