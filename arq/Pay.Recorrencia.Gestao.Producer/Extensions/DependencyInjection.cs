using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pay.Recorrencia.Gestao.Producer.KafkaProducer;
using Pay.Recorrencia.Gestao.Producer.KafkaProducer.Interface;
using Pay.Recorrencia.Gestao.Producer.Models;
using Serilog;

namespace Pay.Recorrencia.Gestao.Producer.Extensions
{
    public static class DependencyInjection
    {

        public static void RegisterKafkaServices<THostedService>(this IServiceCollection services, IConfiguration Configuration)
            where THostedService : class, IHostedService
        {
            var ActivateKafka = Configuration.GetSection("Kafka").GetSection("Producer").GetValue<bool>("Ativar");

            if (!ActivateKafka)
            {
                RegisterFakeKafkaProducerService(services, Configuration);
            }
            else
            {
                RegisterRealKafkaProducerService(services, Configuration);
            }
        }

        public static void RegisterKafkaServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var ActivateKafka = Configuration.GetSection("Kafka").GetSection("Producer").GetValue<bool>("Ativar");

            if (!ActivateKafka)
            {
                Log.Warning("Kafka Producer is disabled");
                RegisterFakeKafkaProducerService(services, Configuration);
            }
            else
            {
                RegisterRealKafkaProducerService(services, Configuration);
            }
        }

        private static void RegisterFakeKafkaProducerService(IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<InputParametersKafkaProducer>(Configuration.GetSection("Kafka"));
            services.AddSingleton<IProducer, ProducerDisabled>();
        }

        private static void RegisterRealKafkaProducerService(IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<InputParametersKafkaProducer>(Configuration.GetSection("Kafka"));
            services.AddSingleton<IProducer, KafkaProducer.Producer>();
        }
    }
}
