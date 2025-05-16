using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pay.Recorrencia.Gestao.Producer.Models;

namespace Pay.Recorrencia.Gestao.Crosscutting.Extensions
{
    public static class SettingsExtension
    {
        public static IServiceCollection AddApiCustomSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InputParametersKafkaProducer>(configuration.GetSection("Kafka"));
            return services;
        }
    }
}
