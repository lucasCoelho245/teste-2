using Confluent.Kafka;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;

namespace Pay.Recorrencia.Gestao.Api.Consumidores
{
    public class TopicCustom : IConsumerOperation
    {
        private readonly ILogger<TopicCustom> _logger;

        public TopicCustom(ILogger<TopicCustom> logger)
        {
            _logger = logger;
        }

        public void Consume(string topic, int partition, string message, Headers headers, string topicWithEnviroment, long offSet)
        {
            /// Regras de Negocio que devem ser feitas quando consumir a mensagem.....

        }
    }
}
