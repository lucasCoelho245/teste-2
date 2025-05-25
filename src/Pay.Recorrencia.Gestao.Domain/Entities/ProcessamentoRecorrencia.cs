namespace Pay.Recorrencia.Gestao.Domain.Entities;

public class ProcessamentoRecorrencia : SolicitacaoRecorrenciaEntrada
{
    public string? MotivoRejeicao { get; set; }
    public string ParticipanteDoUsuarioPagador { get; set; }
    public bool Status { get; set; }
    public string? DataUltimaAtualizacao { get; set; }
    public string SituacaoRecorrencia { get; set; }
    public bool TpJornada {get; set;} = true;
}