using Pay.Recorrencia.Gestao.Consumer.Extensions;
using Pay.Recorrencia.Gestao.Consumer.Models;
using Pay.Recorrencia.Gestao.Consumer.Worker;
using Pay.Recorrencia.Gestao.Crosscutting.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var service = builder.Services;

// Adicionando serviços personalizados e configuração do Kafka
service.AddApiCustomSettings(builder.Configuration);
service.AddWorkerCustomServices(builder.Configuration);
service.AddWorkerMediator(builder.Configuration);

// Configuração do host com serviços
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<InputParametersKafkaConsumer>(context.Configuration.GetSection("Kafka"));
        services.RegisterKafkaConsumerServices(context.Configuration, typeof(Program).Assembly);
        services.AddHostedService<Worker>();
    })
    .Build();

// Inicia o Worker
await host.RunAsync();