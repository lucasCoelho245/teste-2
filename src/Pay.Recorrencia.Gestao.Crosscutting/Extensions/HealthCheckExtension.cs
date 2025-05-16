using Microsoft.Extensions.DependencyInjection;
using Pay.Recorrencia.Gestao.Crosscutting.Health;

namespace Pay.Recorrencia.Gestao.Crosscutting.Extensions
{
    public static class HealthCheckExtension
    {
        public static IServiceCollection AddHealthChecksInjection(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<ExampleHealthCheck>("Example_Healthy");
            return services;
        }

    }
}
