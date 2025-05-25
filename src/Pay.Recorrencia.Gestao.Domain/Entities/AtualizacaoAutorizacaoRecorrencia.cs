using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class AtualizacaoAutorizacaoRecorrencia
    {
        public string? IdAutorizacao { get; set; }
        public string TipoSituacaoRecorrencia { get; set; }
        public string? IdRecorrencia { get; set; }
        public DateTime? DataHoraSituacaoRecorrencia { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}
