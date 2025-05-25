using DataAccess.Domain;
using DataAccess.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Infrastructure.Data;
using Pay.Recorrencia.Gestao.Infrastructure.Data.Interfaces;
using Pay.Recorrencia.Gestao.Infrastructure.Repositories;

namespace Pay.Recorrencia.Gestao.Crosscutting.Extensions
{
    public static class RepositoriesExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConfig = configuration.GetSection(nameof(DBConfig)).Get<DBConfig>();
            var db = new DataAccessFactory(dbConfig);
            services.AddSingleton(db.Create());

            //var dbConfigPixAutomatico = configuration.GetSection("DBConfigPixAutomatico").Get<DBConfig>();
            //services.AddSingleton<IPixAutomaticoDataAccess>(provider => new PixAutomaticoDataAccess(dbConfigPixAutomatico));
            services.AddSingleton<IPixAutomaticoDataAccess>(provider => new PixAutomaticoDataAccess(dbConfig));

            services.AddScoped<ISolicitacaoRecorrenciaRepository, SolicitacaoRecorrenciaRepository>();
            services.AddScoped<IAutorizacaoRecorrenciaRepository, AutorizacaoRecorrenciaRepository>();
            services.AddScoped<IInformacaoSolicitacaoRepository, InformacaoSolicitacaoRepository>();
            services.AddScoped<IControleJornadaRepository, ControleJornadaRepository>();
            services.AddScoped<ISolicitacaoSequencialRepository, SolicitacaoSequencialRepository>();
            services.AddTransient<ISolicitacaoRecorrenciaRepository, SolicitacaoRecorrenciaRepository>();
            services.AddTransient<IMockSolicitacaoRecorrenciaRepository, MockSolicitacaoRecorrenciaRepository>();
            services.AddTransient<IMockAutorizacaoRecorrenciaRepository, MockAutorizacaoRecorrenciaRepository>();
            services.AddTransient<IQRCodeLoactionRepository, QRCodeLocationRepository>();
            services.AddTransient<IJornadaRepository, JornadaRepository>();

            return services;
        }
    }
}
