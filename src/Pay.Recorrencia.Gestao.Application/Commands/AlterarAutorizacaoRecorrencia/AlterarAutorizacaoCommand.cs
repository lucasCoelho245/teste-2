using MediatR;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia
{
    public sealed class AlterarAutorizacaoCommand : IRequest<AutorizacaoRecorrencia>
    {
        public Guid IdAutorizacao { get; set; }
        public decimal ValorMaximoAutorizado { get; set; }
        public bool FlagValorMaximoAutorizado { get; set; }
        public bool FlagPermiteNotificacao { get; set; }
        public string MotivoRejeicaoOcorrencia { get; set; }
        public string CodigoSituacaoCancelamentoRecorrencia { get; set; }
        public string SituacaoRecorrencia { get; set; }
        public string TipoSituacaoRecorrencia { get; set; }
        public DateTime DataHoraSituacaoRecorrencia { get; set; }
    }
}
