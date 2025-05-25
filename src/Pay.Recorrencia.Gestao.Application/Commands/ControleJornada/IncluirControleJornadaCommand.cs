using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.ControleJornada
{
    public class IncluirControleJornadaCommand : IRequest<MensagemPadraoResponse>
    {
        public string? IdE2E { get; set; }
        public string? IdRecorrencia { get; set; }
        public string? TpJornada { get; set; } 
        public string? SituacaoJornada { get; set; }
        public DateTime? DtAgendamento { get; set; }
        public DateTime? DtPagamento { get; set; }
        public DateTime? DataUltimaAtualizacao { get; set; }
    }
}
