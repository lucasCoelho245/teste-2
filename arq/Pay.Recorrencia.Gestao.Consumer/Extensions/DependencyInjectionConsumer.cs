using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pay.Recorrencia.Gestao.Consumer.BackGroundService;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer.Interface;
using System.Reflection;

namespace Pay.Recorrencia.Gestao.Consumer.Extensions
{
    public static class DependencyInjectionConsumer
    {
        public static void RegisterKafkaConsumerServices(
            this IServiceCollection services,
            IConfiguration configuration,
            Assembly applicationAssembly
            )
        {
            RegisterCommonKafkaServices<BackGroundServiceConsumer>(services, configuration, applicationAssembly);
        }

        private static void RegisterCommonKafkaServices<TBackgroundService>(
               IServiceCollection services,
               IConfiguration configuration,
               Assembly applicationAssembly)
               where TBackgroundService : BackgroundService
        {
            Type parentType = typeof(IConsumerOperation);
            List<Type> implementations = applicationAssembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(parentType))
                .ToList();

            services.AddScoped<OperationFallBackConsumer>();
            foreach (Type type in implementations)
            {
                services.AddScoped(type);
            }

            ConsumerCollection consumidoresCollection = new();
            services.AddSingleton(consumidoresCollection);

            services.AddSingleton<ConsumerServices>();
            services.AddSingleton<KafkaConsumer.Consumer>();
            services.AddHostedService<TBackgroundService>();
        }
    }
}
