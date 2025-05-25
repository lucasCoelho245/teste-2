using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Services;

namespace Pay.Recorrencia.Gestao.Infrastructure.Services
{
    public class HttpClient : IHttpClient, IDisposable
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private HttpRequestMessage? _requestMessage;
        private HttpResponseMessage? _responseMessage;
        private readonly string _APIToken;

        public HttpClient(
            IConfiguration config)
        {
            _httpClient = new System.Net.Http.HttpClient();
            _APIToken = config["APIToken"] ?? string.Empty;
            ConfigurarHeaders();
        }

        public void Dispose()
        {
            DisposeRequestResponse();

            if (_httpClient != null)
                _httpClient.Dispose();
        }

        public async Task<RetornoHttpClient> ExecutarRequisicaoAsync<T>(string uri, HttpMethod metodo, T Objeto)
        {
            var retorno = new RetornoHttpClient();

            DisposeRequestResponse();

            try
            {
                _requestMessage = new HttpRequestMessage();
                _requestMessage.Method = metodo;
                _requestMessage.RequestUri = new Uri(uri);
                _requestMessage.Content = JsonContent.Create<T>(Objeto);
                _responseMessage = await _httpClient.SendAsync(_requestMessage);
                retorno.StatusCode = _responseMessage.StatusCode;
                retorno.Content = await _responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar requisição HTTP. " + ex.Message, ex);
            }

            return retorno;
        }

        public async Task<RetornoHttpClient> ExecutarRequisicaoAsync(string uri, HttpMethod metodo, long id = 0)
        {
            var retorno = new RetornoHttpClient();

            DisposeRequestResponse();

            try
            {
                _requestMessage = new HttpRequestMessage();
                _requestMessage.Method = metodo;
                if (id > 0)
                    uri += "/" + id;
                _requestMessage.RequestUri = new Uri(uri);
                _responseMessage = await _httpClient.SendAsync(_requestMessage);
                retorno.StatusCode = _responseMessage.StatusCode;
                retorno.Content = await _responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar requisição HTTP. " + ex.Message, ex);
            }

            return retorno;
        }

        public async Task<RetornoHttpClient> ExecutarRequisicaoAsync(string uri, HttpMethod metodo, (string nome, string valor)[] Parametros)
        {
            var retorno = new RetornoHttpClient();

            DisposeRequestResponse();

            try
            {
                _requestMessage = new HttpRequestMessage();
                _requestMessage.Method = metodo;
                var query = HttpUtility.ParseQueryString(string.Empty);
                foreach (var param in Parametros)
                {
                    query[param.nome] = param.valor;
                }
                uri += "?" + query.ToString();
                _requestMessage.RequestUri = new Uri(uri);
                _responseMessage = await _httpClient.SendAsync(_requestMessage);
                retorno.StatusCode = _responseMessage.StatusCode;
                retorno.Content = await _responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar requisição HTTP. " + ex.Message, ex);
            }

            return retorno;
        }

        public async Task<RetornoHttpClient> ExecutarRequisicaoAsync<T>(string uri, HttpMethod metodo, int id, T Objeto)
        {
            var retorno = new RetornoHttpClient();

            DisposeRequestResponse();

            try
            {
                _requestMessage = new HttpRequestMessage();
                _requestMessage.Method = metodo;
                if (id > 0)
                    uri += id;
                _requestMessage.RequestUri = new Uri(uri);
                _requestMessage.Content = JsonContent.Create<T>(Objeto);
                _responseMessage = await _httpClient.SendAsync(_requestMessage);
                retorno.StatusCode = _responseMessage.StatusCode;
                retorno.Content = await _responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar requisição HTTP. " + ex.Message, ex);
            }

            return retorno;
        }

        public async Task<RetornoHttpClient> ExecutarRequisicaoAsync(HttpRequestMessage request)
        {
            var retorno = new RetornoHttpClient();

            DisposeRequestResponse();

            try
            {
                _responseMessage = await _httpClient.SendAsync(request);
                retorno.StatusCode = _responseMessage.StatusCode;
                retorno.Content = await _responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar requisição HTTP. " + ex.Message, ex);
            }

            return retorno;
        }

        #region Métodos Privados

        private void DisposeRequestResponse()
        {
            if (_responseMessage != null)
                _responseMessage.Dispose();

            if (_requestMessage != null)
                _requestMessage.Dispose();
        }

        private void ConfigurarHeaders()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _APIToken);
        }

        #endregion
    }
}
