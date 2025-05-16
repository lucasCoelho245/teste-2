using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer;

namespace Pay.Recorrencia.Gestao.Api.Consumidores
{
    public static class ConfigConsumerExtension
    {
        public static void ConfigureConsumer(this IApplicationBuilder services, ConsumerServicesMapper consumerServicesMapper)
        {
            consumerServicesMapper
                .MapToFallback<OperationFallBackConsumer>()
                .MapToTopicTransaction<TopicCustom>("TOPICO_AQUI");
        }
    }
}
