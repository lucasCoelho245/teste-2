using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Interfaces
{
    public interface IAtualizarAutorizacaoRecorrenciaService
    {
        Task<MensagemPadraoResponse> Handle(AtualizarSolicitacaoRecorrenciaCommand request);
    }
}