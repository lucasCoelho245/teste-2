using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Lista
{
    public class ListaAutorizacaoRecRequest : GetListaAutorizacaoRecDTOPaginada, IRequest<ListaAutorizacaoRecResponse>
    {}
}