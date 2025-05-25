namespace Pay.Recorrencia.Gestao.Api.Filters
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class AddHeadersOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            // Adicionando todos os cabeçalhos mencionados
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-correlation-id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "ID de correlação único para rastreamento de requisições",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-idempotency-id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "ID de idempotência único para evitar duplicação de requisições",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "authentication", // o nome correto do header é Authorization. Correcao abaixo
                In = ParameterLocation.Header,
                Required = true,
                Description = "Token de autenticação Bearer para acesso seguro",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Token de autenticação Bearer para acesso seguro",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "process-start-time",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Hora de início do processo no formato ISO 8601",
                Schema = new OpenApiSchema { Type = "string", Format = "date-time" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "device-os",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Sistema operacional do dispositivo que está fazendo a requisição",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "device-type",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Tipo de dispositivo (ex.: Laptop, Smartphone, etc.)",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "device-ip",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Endereço IP do dispositivo que está fazendo a requisição",
                Schema = new OpenApiSchema { Type = "string", Format = "ipv4" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "geolocation",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Geolocalização do dispositivo no formato 'latitude,longitude'",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "ispb",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Código ISPB (Identificador de Sistema de Pagamentos Brasileiro)",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "app-user-id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "UserId do App",
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}
