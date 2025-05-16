using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pay.Recorrencia.Gestao.Crosscutting.Extensions
{
    public static class MediatorExtension
    {
        public static IServiceCollection AddWorkerMediator(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = AppDomain.CurrentDomain.Load("Pay.Recorrencia.Gestao.Application");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IMediator).Assembly));

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("Pay.Recorrencia.Gestao.Application");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            return services;
        }

    }
}
