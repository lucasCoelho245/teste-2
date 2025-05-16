using MediatR;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Detalhes
{
    public class DetalhesAutorizacaoRecRequest : GetAutorizacaoRecDTOPaginada, IRequest<DetalhesAutorizacaoRecResponse>
    {}
}