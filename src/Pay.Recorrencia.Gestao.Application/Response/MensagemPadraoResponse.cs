using System.Text.Json.Serialization;

namespace Pay.Recorrencia.Gestao.Application.Response
{
    public class MensagemPadraoResponse
    {
        public string? Status { get; set; }
        public int StatusCode { get; set; }
        public Erro Error { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IdAutorizacaoResponse { get; set; }


        public MensagemPadraoResponse(int statusCode, string codigoInterno, string mensagemErro, string? idAutorizacao = null)
        {
            Status = statusCode.Equals(200) ? "OK" : "NOK";
            StatusCode = statusCode;
            Error = new Erro
            {
                Code = codigoInterno,
                Message = mensagemErro
            };
            IdAutorizacaoResponse = idAutorizacao;
        }

    }


    public class Erro
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
    }
}
