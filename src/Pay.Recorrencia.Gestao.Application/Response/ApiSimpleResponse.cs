namespace Pay.Recorrencia.Gestao.Application.Response
{
    public class ApiSimpleResponse
    {
        public ApiSimpleResponse(string codigoRetorno, string mensagemRetorno)
        {
            this.CodigoRetorno = codigoRetorno;
            this.MensagemRetorno = mensagemRetorno;
        }

        public string CodigoRetorno { get; set; }
        public string MensagemRetorno { get; set; }
    }
}
