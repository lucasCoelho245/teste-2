using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Lista
{
    public class ListaSolicAutorizacaoRecRequest : GetListaSolicAutorizacaoRecDTOPaginada, IRequest<ListaSolicAutorizacaoRecResponse>
    { }
}