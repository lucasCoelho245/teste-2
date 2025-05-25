namespace Pay.Recorrencia.Gestao.Domain.Events
{
    public class ControleJornadaEvent
    {
        public string TpJornada { get; set; } = "Jornada 1";
        public string? IdRecorrencia { get; set; }
        public string SituacaoJornada { get; set; } = "Solicitação Recebida";
        public string? DataUltimaAtualizacao { get; set; }
    }
}
