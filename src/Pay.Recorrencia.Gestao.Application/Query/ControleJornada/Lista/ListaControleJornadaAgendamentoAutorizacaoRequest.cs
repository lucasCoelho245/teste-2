using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista
{
    public class ListaControleJornadaAgendamentoAutorizacaoRequest : PaginacaoDTO, IRequest<ListaControleJornadaResponse>
    {
        public required string TpJornada { get; set; }
        public string? IdRecorrencia { get; set; }
        public string? IdE2E { get; set; }
        public string? IdConciliacaoRecebedor { get; set; }
    }
}
