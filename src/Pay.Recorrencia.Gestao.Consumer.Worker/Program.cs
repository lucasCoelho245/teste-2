using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Services;
using Pay.Recorrencia.Gestao.Consumer.Extensions;
using Pay.Recorrencia.Gestao.Consumer.Models;
using Pay.Recorrencia.Gestao.Consumer.Worker;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.PayPagamentoProcessado.Validation.IValidation;
using Pay.Recorrencia.Gestao.Crosscutting.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Registro do AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Kafka Consumer
        services.Configure<InputParametersKafkaConsumer>(context.Configuration.GetSection("Kafka"));

        // Configurações de API
        services.AddApiCustomSettings(context.Configuration);
        services.AddApiCustomServices(context.Configuration);

        // Configuração do Service
        services.AddScoped<IAtualizarAutorizacaoService, AtualizarAutorizacaoService>();
        services.AddScoped<IInserirAutorizacaoRecorrenciaService, InserirAutorizacaoRecorrenciaService>();
        services.AddScoped<IAtualizarAutorizacaoRecorrenciaService, AtualizarAutorizacaoRecorrenciaService>();
        services.AddScoped<IJornadaService, JornadaService>();

        // Kafka Consumer
        services.RegisterKafkaConsumerServices(context.Configuration, typeof(Program).Assembly);

        // Worker e serviços customizados
        services.AddWorkerCustomServices(context.Configuration);
        services.AddMediator();
        services.AddRepository(context.Configuration);

        // Cadeia de validação (Chain of Responsibility)
        services.AddScoped<ValidadorExistenciaRecorrencia>();
        services.AddScoped(sp =>
            new ValidadorDadosConta(
                sp.GetRequiredService<ValidadorExistenciaRecorrencia>(),
                sp.GetRequiredService<ILogger<ValidadorDadosConta>>()
            )
        );
        services.AddScoped(sp =>
            new ValidadorDadosEntrada(
                sp.GetRequiredService<ValidadorDadosConta>(),
                sp.GetRequiredService<ILogger<ValidadorDadosEntrada>>()
            )
        );
        services.AddScoped<IValidacaoSolicitacao>(sp =>
            sp.GetRequiredService<ValidadorDadosEntrada>()
        );

        // Worker
        services.AddHostedService<Worker>();
    })
    .Build();


await host.RunAsync();
