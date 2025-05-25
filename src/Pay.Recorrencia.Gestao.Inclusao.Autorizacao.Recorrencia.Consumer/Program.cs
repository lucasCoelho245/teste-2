using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pay.Recorrencia.Gestao.Consumer.Extensions;
using Pay.Recorrencia.Gestao.Consumer.Models;
using Pay.Recorrencia.Gestao.Crosscutting.Extensions;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Inclusao.Autorizacao.Recorrencia.Consumer;
using Pay.Recorrencia.Gestao.Infrastructure.Services;
using Pay.Recorrencia.Gestao.Producer.KafkaProducer;
using Pay.Recorrencia.Gestao.Producer.KafkaProducer.Interface;
using Pay.Recorrencia.Gestao.Producer.Models;

var builder = Host.CreateApplicationBuilder(args);
var service = builder.Services;

// Configuração do host com serviços
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<InputParametersKafkaConsumer>(context.Configuration.GetSection("Kafka"));
        services.Configure<InputParametersKafkaProducer>(context.Configuration.GetSection("Kafka"));
        services.RegisterKafkaConsumerServices(context.Configuration, typeof(Program).Assembly);

        services.AddScoped<IKafkaProducerService, KafkaProducerService>();
        services.AddSingleton<IProducer, Producer>();

        services.AddAutoMapper(typeof(Program));

        services.AddApiCustomSettings(context.Configuration);
        services.AddApiCustomServices(context.Configuration);

        services.AddWorkerCustomServices(context.Configuration);
        services.AddMediator();
        services.AddRepository(context.Configuration);

        services.AddHostedService<Worker>();
    })
    .Build();

// Inicia o Worker
await host.RunAsync();