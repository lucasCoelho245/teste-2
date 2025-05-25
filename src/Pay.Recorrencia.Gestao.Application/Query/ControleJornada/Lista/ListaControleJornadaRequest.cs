using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista
{
    public class ListaControleJornadaRequest : PaginacaoDTO, IRequest<ListaControleJornadaResponse>
    { }
}
