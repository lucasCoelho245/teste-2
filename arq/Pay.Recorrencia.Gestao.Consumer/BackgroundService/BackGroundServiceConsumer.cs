using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer;
using Pay.Recorrencia.Gestao.Consumer.Models;

namespace Pay.Recorrencia.Gestao.Consumer.BackGroundService
{
    public class BackGroundServiceConsumer : BackGroundServiceBaseConsumer
    {
        public BackGroundServiceConsumer(
            IOptions<InputParametersKafkaConsumer> inputParameterskafka,
            ILogger<BackGroundServiceConsumer> logger,
            ConsumerServices consumerServices,
            IServiceScopeFactory scopeFactory) : base(inputParameterskafka, logger, consumerServices, scopeFactory)
        {
        }

        protected override async Task<ConsumeResult<Null, string>> ConsumeMessage(IConsumer<Null, string> consumidor, CancellationToken token)
        {
            return await consumidor.ConsumeAsync(token);
        }
    }
}
