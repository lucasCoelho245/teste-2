using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Domain.Services
{
    public interface IHttpClient
    {
        /// <summary>
        /// Executa uma requisição HTTP assíncrona com o corpo da requisição contendo o Objeto informado em formato JSON.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="metodo"></param>
        /// <param name="Objeto"></param>
        /// <returns></returns>
        public Task<RetornoHttpClient> ExecutarRequisicaoAsync<T>(string uri, HttpMethod metodo, T Objeto);
        /// <summary>
        /// Executa uma requisição HTTP assíncrona com o id informado na url.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="metodo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<RetornoHttpClient> ExecutarRequisicaoAsync(string endpoint, HttpMethod metodo, long id = 0);
        /// <summary>
        /// Executa uma requisição HTTP assíncrona com o corpo da requisição contendo os parâmetros informados na url.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="metodo"></param>
        /// <param name="Parametros"></param>
        /// <returns></returns>
        public Task<RetornoHttpClient> ExecutarRequisicaoAsync(string uri, HttpMethod metodo, (string nome, string valor)[] Parametros);
        /// <summary>
        /// Executa uma requisição HTTP assíncrona com o id informado na url e com o corpo da requisição contendo o objeto informado em formato JSON.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="metodo"></param>
        /// <param name="id"></param>
        /// <param name="Objeto"></param>
        /// <returns></returns>
        public Task<RetornoHttpClient> ExecutarRequisicaoAsync<T>(string uri, HttpMethod metodo, int id, T Objeto);
        /// <summary>
        /// Executa uma requisição HTTP assíncrona com a mensagem passada.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<RetornoHttpClient> ExecutarRequisicaoAsync(HttpRequestMessage request);
    }
}
