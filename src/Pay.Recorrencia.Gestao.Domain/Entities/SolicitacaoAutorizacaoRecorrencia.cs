namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    /// <summary>
    /// Classe que representa a entidade de solicitação de autorização de recorrência.
    /// </summary>
    public class SolicitacaoAutorizacaoRecorrencia
    {
        public string IdRecorrencia { get; set; }

        public string TipoRecorrencia { get; set; }

        public string TipoFrequencia { get; set; }

        public DateTime DataInicialRecorrencia { get; set; }

        public DateTime? DataFinalRecorrencia { get; set; }

        public string? CodigoMoedaSolicRecorr { get; set; }

        public decimal? ValorFixoSolicRecorrencia { get; set; }

        public bool IndicadorValorMin { get; set; }

        public decimal? ValorMinRecebedorSolicRecorr { get; set; }

        public string NomeUsuarioRecebedor { get; set; }

        public string CpfCnpjUsuarioRecebedor { get; set; }

        public string ParticipanteDoUsuarioRecebedor { get; set; }

        public string CpfCnpjUsuarioPagador { get; set; }

        public string ParticipanteDoUsuarioPagador { get; set; }

        public int ContaUsuarioPagador { get; set; }

        public int? AgenciaUsuarioPagador { get; set; }

        public string? NomeDevedor { get; set; }

        public string? CpfCnpjDevedor { get; set; }

        public string NumeroContrato { get; set; }

        public string? DescObjetoContrato { get; set; }

        public string TpRetentativa { get; set; }

        public DateTime DataHoraCriacaoRecorr { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }
    }
    public class SolicitacaoAutorizacaoRecorrenciaDetalhes
    {
        public string IdRecorrencia { get; set; }
        public string TipoRecorrencia { get; set; }
        public string TipoFrequencia { get; set; }
        public DateTime DataInicialRecorrencia { get; set; }
        public DateTime? DataFinalRecorrencia { get; set; }
        public string? CodigoMoedaSolicRecorr { get; set; }
        public decimal? ValorFixoSolicRecorrencia { get; set; }
        public string IndicadorValorMin { get; set; }
        public decimal? ValorMinRecebedorSolicRecorr { get; set; }
        public string NomeUsuarioRecebedor { get; set; }
        public string CpfCnpjUsuarioRecebedor { get; set; }
        public string ParticipanteDoUsuarioRecebedor { get; set; }
        public string CpfCnpjUsuarioPagador { get; set; }
        public string ParticipanteDoUsuarioPagador { get; set; }
        public int ContaUsuarioPagador { get; set; }
        public int? AgenciaUsuarioPagador { get; set; }
        public string? NomeDevedor { get; set; }
        public string? CpfCnpjDevedor { get; set; }
        public string NumeroContrato { get; set; }
        public string? DescObjetoContrato { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public string MotivoRejeicao { get; set; }
        public DateTime DataHoraCriacaoRecorr { get; set; }
        public DateTime DataHoraCriacaoSolicRecorr { get; set; }
        public DateTime DataHoraExpiracaoSolicRecorr { get; set; }
        public string CodigoSituacaoCancelamentoRecorrencia { get; set; }
        public string SituacaoSolicRecorrencia { get; set; }
    }
}