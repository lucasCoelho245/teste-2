using System.Net;

namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    public class RetornoHttpClient
    {
        public RetornoHttpClient()
        {
            Content = string.Empty;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string Content { get; set; }
    }
}
