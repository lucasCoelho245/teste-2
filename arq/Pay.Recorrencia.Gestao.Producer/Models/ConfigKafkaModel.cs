using Confluent.Kafka;

namespace Pay.Recorrencia.Gestao.Producer.Models
{
    public class ConfigKafkaModel
    {
        public ConfigKafkaModel() { }
        public ProducerConfig ProduceConfigInit(string bootstrapServers, SaslMechanism saslMechanism,
            SecurityProtocol securityProtocol,
            string sslCaLocation,
            string saslUsername,
            string saslPassword)
        {
            return new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                SaslMechanism = saslMechanism,
                SecurityProtocol = securityProtocol,
                SslCaLocation = string.IsNullOrEmpty(sslCaLocation) ? @"/app/ca.crt" : sslCaLocation,
                SaslUsername = saslUsername,
                SaslPassword = saslPassword,
                Partitioner = Partitioner.Random
            };
        }
    }
}
