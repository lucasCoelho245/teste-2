namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    public class JornadaAutorizacaoAgendamentoDTO : PaginacaoDTO
    {
        public string? TpJornada { get; set; }
        public string? IdRecorrencia { get; set; }
        public string? IdE2E { get; set; }
        public string? IdConciliacaoRecebedor { get; set; }
    }
}
