namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class ControleJornadaEntrada
    {
        public required string TpJornada { get; set; }
        public required string IdRecorrencia { get; set; }
        public string? IdFimAFim { get; set; }
        public string? IdConciliacaoRecebedor { get; set; }
        public string? SituacaoJornada { get; set; }
        public DateTime? DtAgendamento { get; set; }
        public decimal? VlAgendamento { get; set; }
        public DateTime? DtPagamento { get; set; }
        public DateTime? DataUltimaAtualizacao { get; set; }
    }
}
