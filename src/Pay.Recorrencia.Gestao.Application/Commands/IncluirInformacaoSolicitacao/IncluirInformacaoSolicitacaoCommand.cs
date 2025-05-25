using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.IncluirInformacaoSolicitacao
{
    public class IncluirInformacaoSolicitacaoRequest(string ispb) : IRequest<long>
    {
        public string Ispb { get; } = ispb;
    }
}
