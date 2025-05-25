using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Infrastructure.Services;
using Pay.Recorrencia.Gestao.Producer.Extensions;
using HttpClient = Pay.Recorrencia.Gestao.Infrastructure.Services.HttpClient;

namespace Pay.Recorrencia.Gestao.Crosscutting.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddApiCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            DependencyInjections.RegisterKafkaServices(services, configuration);
            services.AddScoped<IKafkaProducerService, KafkaProducerService>();
            services.AddTransient<IHttpClient, HttpClient>();
            services.AddTransient<IPushService, PushService>();
            return services;
        }
        public static IServiceCollection AddWorkerCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
            return services;
        }
    }
}
