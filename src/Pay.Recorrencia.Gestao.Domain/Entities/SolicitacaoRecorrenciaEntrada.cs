namespace Pay.Recorrencia.Gestao.Domain.Entities
{
    /// <summary>
    /// Classe que representa a entidade de solicitação de recorrência providenciada pelo AutBack/CRK através do arquivo PAIN009.
    /// </summary>
    public class SolicitacaoRecorrenciaEntrada
    {
        public required string IdSolicRecorrencia { get; set; }
        public required string IdRecorrencia { get; set; }
        public required string TipoFrequencia { get; set; }
        public required string DataInicialRecorrencia { get; set; }
        public string? DataFinalRecorrencia { get; set; }
        public string? ValorFixoSolicRecorrencia { get; set; }
        public string? ValorMinRecebedorSolicRecorr { get; set; }
        public required string NomeUsuarioRecebedor { get; set; }
        public required string CpfCnpjUsuarioRecebedor { get; set; }
        public required string ParticipanteDoUsuarioRecebedor { get; set; }
        public required string CpfCnpjUsuarioPagador { get; set; }
        public required string ContaUsuarioPagador { get; set; }
        public string? AgenciaUsuarioPagador { get; set; }
        public string? NomeDevedor { get; set; }
        public string? CpfCnpjDevedor { get; set; }
        public required string NumeroContrato { get; set; }
        public string? DescObjetoContrato { get; set; }
        public required string DataHoraCriacaoRecorr { get; set; }
        public required string DataHoraCriacaoSolicRecorr { get; set; }
        public required string DataHoraExpiracaoSolicRecorr { get; set; }
    }
}