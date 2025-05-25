using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia
{
    public sealed class AlterarAutorizacaoCommand : IRequest<MensagemPadraoResponse>
    {
        public string? IdAutorizacao { get; set; }
        public string? IdRecorrencia { get; set; }
        public string? SituacaoRecorrencia { get; set; } //Enum
        public string? TipoRecorrencia { get; set; } //Enum
        public string? TipoFrequencia { get; set; } //Enum
        public DateTime DataInicialAutorizacaoRecorrencia { get; set; }
        public DateTime? DataFinalAutorizacaoRecorrencia { get; set; }
        public string? CodigoMoedaAutorizacaoRecorrencia { get; set; } //Enum
        public decimal? ValorRecorrencia { get; set; }
        public decimal? ValorMaximoAutorizado { get; set; }
        public string? MotivoRejeicaoRecorrencia { get; set; } //Enum
        public string? NomeUsuarioRecebedor { get; set; }
        public string? CpfCnpjUsuarioRecebedor { get; set; }
        public string? ParticipanteDoUsuarioRecebedor { get; set; }
        public string? CodMuniIBGE { get; set; }
        public string? CpfCnpjUsuarioPagador { get; set; }
        public string? ContaUsuarioPagador { get; set; }
        public string? AgenciaUsuarioPagador { get; set; }
        public string? ParticipanteDoUsuarioPagador { get; set; }
        public string? NomeDevedor { get; set; }
        public string? CpfCnpjDevedor { get; set; }
        public string? NumeroContrato { get; set; }
        public string? DescObjetoContrato { get; set; }
        public string? CodigoSituacaoCancelamentoRecorrencia { get; set; }  //Enum
        public string TipoSituacaoRecorrencia { get; set; }  //Enum
        public DateTime? DataHoraCriacaoRecorr { get; set; }
        public bool? FlagPermiteNotificacao { get; set; }
        public bool? FlagValorMaximoAutorizado { get; set; }
        public string? TpRetentativa { get; set; }  //Enum
        public DateTime? DataProximoPagamento { get; set; }
        public DateTime? DataHoraSituacaoRecorrencia { get; set; }
    }
}
