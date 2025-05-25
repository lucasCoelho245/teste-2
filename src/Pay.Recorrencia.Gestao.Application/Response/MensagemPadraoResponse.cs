namespace Pay.Recorrencia.Gestao.Application.Response
{
    public class MensagemPadraoResponse
    {
        public MensagemPadraoResponse(int statusCode, string codigoInterno, string mensagemErro)
        {
            Status = statusCode.Equals(200) ? "OK" : "NOK";
            StatusCode = statusCode;
            Error = new Erro
            {
                Code = codigoInterno,
                Message = mensagemErro
            };
        }

        public string? Status { get; set; }
        public int StatusCode { get; set; }
        public Erro Error { get; set; }
    }


    public class Erro
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
    }
}
