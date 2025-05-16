using AutoMapper;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Crosscutting.Extensions;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System.Text;
using System.Text.Json;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurar servi�os
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configurar o pipeline de middleware
        ConfigureMiddleware(app);

        app.Run();
    }

    /// <summary>
    /// Configura os servi�os da aplica��o.
    /// </summary>
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(Program));
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IncluirSolicitacaoRecorrenciaCommand, SolicitacaoRecorrencia>();
            cfg.CreateMap<AtualizarSolicitacaoRecorrenciaCommand, SolicitacaoAutorizacaoRecorrenciaUpdateDTO>();
        });
        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);

        // Controllers e API
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecksInjection();

        // Servi�os customizados
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
            });
        services.AddApiCustomSettings(configuration);
        services.AddApiCustomServices(configuration);
        /*
        // Configura��o de valida��o de modelo
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

                var response = new MensagemPadraoResponse
                {
                    CodigoRetorno = "ERRO-PIXAUTO-002",
                    MensagemErro = "Campos não preenchidos corretamente"
                };

                return new BadRequestObjectResult(new
                {
                    response.CodigoRetorno,
                    response.MensagemErro
                });
            };
        });
        */
    }

    /// <summary>
    /// Configura o pipeline de middleware da aplica��o.
    /// </summary>
    private static void ConfigureMiddleware(WebApplication app)
    {
        // Configura��o do Swagger
        if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Homolog"))
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

        // Middlewares padr�o
        app.UseHttpsRedirection();
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
                        jsonWriter.WriteString("status", healthEntry.Value.Description);
                    }
                    jsonWriter.WriteEndObject();
                }

                return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
            }
        });

        // Iniciando o Consumidor 
        //ConsumerServicesMapper consumerServicesMapperOptions = new ConsumerServicesMapper(app.Services);
        //app.ConfigureConsumer(consumerServicesMapperOptions);

        app.Run();
    }
}
