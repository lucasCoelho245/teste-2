
namespace Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia
{
    public interface IInserirAutorizacaoRecorrenciaCommand
    {
        int? AgenciaUsuarioPagador { get; set; }
        string? CodigoMoedaAutorizacaoRecorrencia { get; set; }
        string CodigoSituacaoCancelamentoRecorrencia { get; set; }
        string? CodMunIBGE { get; set; }
        int ContaUsuarioPagador { get; set; }
        string? CpfCnpjDevedor { get; set; }
        string CpfCnpjUsuarioPagador { get; set; }
        string CpfCnpjUsuarioRecebedor { get; set; }
        DateTime? DataAutorizacao { get; set; }
        DateTime? DataCancelamento { get; set; }
        DateTime? DataFinalAutorizacaoRecorrencia { get; set; }
        DateTime DataHoraCriacaoRecorr { get; set; }
        DateTime DataInicialAutorizacaoRecorrencia { get; set; }
        DateTime? DataProximoPagamento { get; set; }
        DateTime DataUltimaAtualizacao { get; set; }
        string? DescObjetoContrato { get; set; }
        bool FlagPermiteNotificacao { get; set; }
        bool FlagValorMaximoAutorizado { get; set; }
        string IdRecorrencia { get; set; }
        string MotivoRejeicaoRecorrencia { get; set; }
        string? NomeDevedor { get; set; }
        string NomeUsuarioRecebedor { get; set; }
        string NumeroContrato { get; set; }
        string ParticipanteDoUsuarioPagador { get; set; }
        string ParticipanteDoUsuarioRecebedor { get; set; }
        string SituacaoRecorrencia { get; set; }
        string TipoFrequencia { get; set; }
        string TipoRecorrencia { get; set; }
        string TipoSituacaoRecorrencia { get; set; }
        string TpRetentativa { get; set; }
        decimal? ValorMaximoAutorizado { get; set; }
        decimal? ValorRecorrencia { get; set; }
    }
}