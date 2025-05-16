using MediatR;

namespace Pay.Recorrencia.Gestao.Application.Query.Notifications.KafkaConsumer
{
    public class KafkaConsumerNotification : INotification
    {
        public string Topic { get; set; }
        public string Message { get; set; }

        public KafkaConsumerNotification(string topic, string message)
        {
            Topic = topic;
            Message = message;
        }
    }
}
