namespace Pay.Recorrencia.Gestao.Application.Response;

public class MensagemRecorrenciaResponse
{
    public string IdRecorrencia { get; set; }
    public string IdInformacaoCancelamento { get; set; }
    public List<ErroRecorrencia> Erros { get; set; } = new List<ErroRecorrencia>();
    public bool OK { get; set; }

}

public class ErroRecorrencia
{
    public int Code { get; set; }
    public string Msg { get; set; }
}
