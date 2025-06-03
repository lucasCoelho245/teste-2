namespace Pay.Recorrencia.Gestao.Domain.DTO;

public class RecorrenciaUnificadaDto
{
     #region Identificadores
        public string IdSolicRecorrencia { get; set; }
        public string IdAutorizacao { get; set; }
        public string IdRecorrencia { get; set; }
        #endregion

        #region Informações da Recorrência
        public string TipoRecorrencia { get; set; }
        public string TipoFrequencia { get; set; }
        public string SituacaoRecorrencia { get; set; }
        public string SituacaoSolicRecorrencia { get; set; }
        #endregion

        #region Datas
        public DateTime DataInicialRecorrencia { get; set; }
        public DateTime? DataFinalRecorrencia { get; set; }
        public DateTime DataInicialAutorizacaoRecorrencia { get; set; }
        public DateTime? DataFinalAutorizacaoRecorrencia { get; set; }
        public DateTime DataHoraCriacaoRecorr { get; set; }
        public DateTime DataHoraCriacaoSolicRecorr { get; set; }
        public DateTime? DataHoraExpiracaoSolicRecorr { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public DateTime? DataProximoPagamento { get; set; }
        #endregion

        #region Valores e Moeda
        public string CodigoMoedaSolicRecorr { get; set; }
        public string CodigoMoedaAutorizacaoRecorrencia { get; set; }
        public decimal ValorRecorrencia { get; set; }
        public decimal ValorFixoSolicRecorrencia { get; set; }
        public decimal? ValorMaximoAutorizado { get; set; }
        public bool IndicadorValorMin { get; set; }
        public decimal? ValorMinRecebedorSolicRecorr { get; set; }
        #endregion

        #region Informações do Recebedor
        public string NomeUsuarioRecebedor { get; set; }
        public string CpfCnpjUsuarioRecebedor { get; set; }
        public string ParticipanteDoUsuarioRecebedor { get; set; }
        #endregion

        #region Informações do Pagador
        public string CpfCnpjUsuarioPagador { get; set; }
        public string ContaUsuarioPagador { get; set; }
        public string AgenciaUsuarioPagador { get; set; }
        public string ParticipanteDoUsuarioPagador { get; set; }
        #endregion

        #region Informações do Devedor
        public string NomeDevedor { get; set; }
        public string CpfCnpjDevedor { get; set; }
        #endregion

        #region Informações do Contrato
        public string NumeroContrato { get; set; }
        public string DescObjetoContrato { get; set; }
        public string CodMunIBGE { get; set; }
        #endregion

        #region Status e Flags
        public string MotivoRejeicao { get; set; }
        public string MotivoRejeicaoRecorrencia { get; set; }
        public string CodigoSituacaoCancelamentoRecorrencia { get; set; }
        public bool FlagPermiteNotificacao { get; set; }
        public bool FlagValorMaximoAutorizado { get; set; }
        public string TpRetentativa { get; set; }
        #endregion

}