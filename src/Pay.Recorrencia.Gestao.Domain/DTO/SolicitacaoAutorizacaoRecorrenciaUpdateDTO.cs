namespace Pay.Recorrencia.Gestao.Domain.DTO
{
    public class SolicitacaoAutorizacaoRecorrenciaUpdateDTO
    {
        public required string IdSolicRecorrencia { get; set; }

        public string? IdAutorizacao { get; set; }

        public string? SituacaoSolicRecorrencia { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }
        public string CodigoSituacaoCancelamentoRecorrencia { get; set; }
    }
}
