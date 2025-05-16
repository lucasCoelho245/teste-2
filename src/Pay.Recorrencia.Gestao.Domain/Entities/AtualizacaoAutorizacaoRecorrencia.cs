using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    public class AtualizacaoAutorizacaoRecorrencia
    {
        public Guid IdAutorizacao { get; set; }
        public TipoSituacaoRecorrencia TipoSituacaoRecorrencia { get; set; }
        public Guid IdRecorrencia { get; set; }
        public DateTime DataHoraSituacaoRecorrencia { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}
