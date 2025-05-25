using AutoMapper;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Pay.Recorrencia.Gestao.Api.Filters;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.ConfirmacaoAutorizacaoRecorr;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Detalhes;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Application.Services;
using Pay.Recorrencia.Gestao.Crosscutting.Extensions;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Domain.Settings;
using Pay.Recorrencia.Gestao.Infrastructure.Repositories;
using System.Data;
using System.Text;
using System.Text.Json;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurar serviços
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configurar o pipeline de middleware
        ConfigureMiddleware(app);

        app.Run();
    }

    /// <summary>
    /// Configura os serviços da aplicação.
    /// </summary>
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // AutoMapper
        // Registro do AutoMapper
        services.AddAutoMapper(typeof(Program));

        // Controllers e API
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecksInjection(configuration["DBConfig:ConnectionString"],
                                          configuration["Kafka:BootstrapServers"]);

        // Serviços customizados
        services.AddMediator();
        services.AddRepository(configuration);
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Pix Recorrente Gestao",
                    Description = "Api para tratamento do pix automático"

                });
                c.OperationFilter<AddHeadersOperationFilter>();
            });
        services.AddApiCustomSettings(configuration);
        services.AddApiCustomServices(configuration);

        ConfigureServiceApplication(services);

        // --- Fim das minhas adições ---


        // Configuração de validação de modelo
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors?.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value?.Errors.Select(e => e.ErrorMessage)
                    });

                var response = new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-002", "Campos não preenchidos corretamente.");

                return new BadRequestObjectResult(response);
            };
        });
        services.Configure<KafkaTopics>(configuration.GetSection("kafka"));
    }

    /// <summary>
    /// Configura o pipeline de middleware da aplicação.
    /// </summary>
    private static void ConfigureMiddleware(WebApplication app)
    {
        // Configuração do Swagger
        if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Homolog") || app.Environment.IsEnvironment("Alpha"))
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = "swagger";
            });
        }

        // Middlewares padrão
        //app.UseHttpsRedirection();
        app.UseAuthorization();

        // Mapear controllers
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = (context, health) =>
            {
                context.Response.ContentType = "application/json";
                var options = new JsonWriterOptions { Indented = true };
                using var memoryStream = new MemoryStream();
                using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
                {
                    jsonWriter.WriteStartObject();
                    foreach (var healthEntry in health.Entries)
                    {
                        jsonWriter.WriteStartObject(healthEntry.Key);
                        jsonWriter.WriteString("status", healthEntry.Value.Status.ToString());
                        jsonWriter.WriteString("description", healthEntry.Value.Status.ToString().Equals("Healthy") ? "UP" : healthEntry.Value.Description ?? "");
                        jsonWriter.WriteString("exception", healthEntry.Value.Exception?.Message ?? "");
                        jsonWriter.WriteEndObject();
                    }
                    //jsonWriter.WriteEndObject();
                }

                return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
            }
        });

        // Iniciando o Consumidor 
        //ConsumerServicesMapper consumerServicesMapperOptions = new ConsumerServicesMapper(app.Services);
        //app.ConfigureConsumer(consumerServicesMapperOptions);

        app.Run();
    }

    private static void ConfigureServiceApplication(IServiceCollection services)
    {
        services.AddScoped<IJornadaService, JornadaService>();
        services.AddScoped<IAtualizarAutorizacaoService, AtualizarAutorizacaoService>();
        services.AddScoped<IInserirAutorizacaoRecorrenciaService, InserirAutorizacaoRecorrenciaService>();
        services.AddScoped<IAtualizarAutorizacaoRecorrenciaService, AtualizarAutorizacaoRecorrenciaService>();
    }
}

