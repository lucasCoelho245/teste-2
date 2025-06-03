namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    public class PixAgendamentoDTO
    {
        public string IdOperacao { get; set; }
        public string IdRecorrencia { get; set; }
        public decimal VlOperacao { get; set; }
        public DateTime DtPagto { get; set; }
        public string NomeUsuarioRecebedor { get; set; }
    }
}
