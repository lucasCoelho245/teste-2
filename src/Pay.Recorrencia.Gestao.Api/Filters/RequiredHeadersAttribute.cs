using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pay.Recorrencia.Gestao.Application.Response;
namespace Pay.Recorrencia.Gestao.Api.Filters
{
    public partial class RequiredHeadersAttribute : ActionFilterAttribute
    {
        private readonly string[] _headers;

        public RequiredHeadersAttribute(params string[] headers)
        {
            _headers = headers;
        }
        private static bool ValidaCabecalho(string header, string value)
        {
            var regras = new Dictionary<string, bool>
            {
                {
                    "x-correlation-id",
                    Guid.TryParse(value, out Guid xCorrelation)
                },
                {
                    "x-idempotency-id",
                    Guid.TryParse(value, out Guid xIdepotency)
                },
                {
                    "Authorization",
                    value.StartsWith("Bearer ")
                },
                {
                    "process-start-time",
                    DateTimeOffset.TryParseExact(
                        value,
                        "yyyy-MM-ddTHH:mm:ssZ",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AssumeUniversal,
                        out var processStartTime
                    )
                },
                {
                    "device-os",
                    value.Length > 3
                },
                {
                    "device-type",
                    value.Length > 3
                },
                {
                    "device-ip",
                    IPAddress.TryParse(value, out var ipAddress)
                },
                {
                    "geolocation",
                    MyRegex().IsMatch(value)
                },
                {
                    "ispb",
                    value == "61033106" || value == "60779196"
                },
                {
                    "app-user-id",
                    value.Length > 5
                },
            };
            return regras.Where(item => item.Key == header).First().Value;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var faltantes = new List<string>();
            var padroes = new Dictionary<string, bool>
            {
                { "x-correlation-id", false },
                { "x-idempotency-id", false },
                { "Authorization", false },
                { "process-start-time", false },
                { "device-os", false },
                { "device-type", false },
                { "device-ip",false },
                { "geolocation",false },
                { "ispb", false },
                { "app-user-id", false }
            };
            foreach (var header in _headers)
            {
                if (!context.HttpContext.Request.Headers.ContainsKey(header))
                {
                    faltantes.Add($"{header} (Requerido)");
                }
            }
            foreach (var header in padroes.Keys)
            {
                bool atualRequedido = padroes.Where(item => item.Key == header).First().Value;

                if (!context.HttpContext.Request.Headers.ContainsKey(header) && atualRequedido)
                {
                    faltantes.Add($"{header} (Requerido)");
                }
                else if (context.HttpContext.Request.Headers.TryGetValue(header, out var valor))
                {
                    bool valida = ValidaCabecalho(header, valor.ToString());
                    if (!valida)
                    {
                        faltantes.Add($"{header} (Valor inválido)");
                    }
                }
            }
            if (faltantes.Count != 0)
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "application/json",
                    Content = JsonSerializer.Serialize(new ErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Error = new Error
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = $"Cabeçalhos: {string.Join(", ", faltantes)}"
                        }
                    })
                };
                return;
            }

            base.OnActionExecuting(context);
        }

        [GeneratedRegex(@"^-?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*-?(180(\.0+)?|((1[0-7]\d)|(\d{1,2}))(\.\d+)?)$")]
        private static partial Regex MyRegex();
    }
}
