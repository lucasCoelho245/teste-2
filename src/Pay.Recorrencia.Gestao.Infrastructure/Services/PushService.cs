using System.Net;
using Microsoft.Extensions.Configuration;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Services;

namespace Pay.Recorrencia.Gestao.Infrastructure.Services
{
    public class PushService : IPushService
    {
        private readonly IHttpClient _httpClient;
        private readonly string _urlPush;

        public PushService(IHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _urlPush = configuration.GetValue<string>("EndpointPush") ?? string.Empty;
        }

        public async Task<bool> EnviarPush(string titulo, string mensagem, string[]? destinatarios)
        {
            var payload = new EnviarPushDTO() { titlePush = titulo, messagePush = mensagem, destinatarios = destinatarios };
            var response = await _httpClient.ExecutarRequisicaoAsync(_urlPush, HttpMethod.Post, payload);

            if (response == null || response.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }
    }
}
