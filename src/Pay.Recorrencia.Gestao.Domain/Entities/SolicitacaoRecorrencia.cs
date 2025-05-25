namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    /// <summary>
    /// Classe que representa a entidade de solicitação de recorrência providenciada pelo AutBack/CRK através do arquivo PAIN009.
    /// </summary>
    public class SolicitacaoRecorrencia
    {
        public string IdSolicRecorrencia { get; set; }
        public string? IdAutorizacao { get; set; }
        public string IdRecorrencia { get; set; }
        public string TipoRecorrencia { get; set; }
        public string TipoFrequencia { get; set; }
        public string DataInicialRecorrencia { get; set; }
        public string? DataFinalRecorrencia { get; set; }
        public string SituacaoSolicRecorrencia { get; set; }
        public string? CodigoMoedaSolicRecorr { get; set; }
        public string? ValorFixoSolicRecorrencia { get; set; }
        public string IndicadorValorMin { get; set; }
        public string? ValorMinRecebedorSolicRecorr { get; set; }
        public string NomeUsuarioRecebedor { get; set; }
        public string CpfCnpjUsuarioRecebedor { get; set; }
        public string ParticipanteDoUsuarioRecebedor { get; set; }
        public string CpfCnpjUsuarioPagador { get; set; }
        public string ContaUsuarioPagador { get; set; }
        public string? AgenciaUsuarioPagador { get; set; }
        public string? NomeDevedor { get; set; }
        public string? CpfCnpjDevedor { get; set; }
        public string NumeroContrato { get; set; }
        public string? DescObjetoContrato { get; set; }
        public string? DataHoraCriacaoRecorr { get; set; }
        public string DataHoraCriacaoSolicRecorr { get; set; }
        public string DataHoraExpiracaoSolicRecorr { get; set; }
        public string DataUltimaAtualizacao { get; set; }
    }
    public class SolicAutorizacaoRecNonPagination
    {
        public SolicitacaoRecorrencia Data { get; set; }
    }

    public class SolicAutorizacaoRecList
    {
        public string IdSolicRecorrencia { get; set; }
        public string NomeUsuarioRecebedor { get; set; }
        public string SituacaoSolicRecorrencia { get; set; }
    }
    public class ListaSolicAutorizacaoRecPaginada
    {
        public IEnumerable<SolicAutorizacaoRecList> Items { get; set; }
        public int TotalItems { get; set; }
    }
}