using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes
{
    public class DetalhesSolicAutorizacaoRecRequest : GetSolicAutorizacaoRecDTOPaginada, IRequest<DetalhesSolicAutorizacaoRecResponse>
    { }
}