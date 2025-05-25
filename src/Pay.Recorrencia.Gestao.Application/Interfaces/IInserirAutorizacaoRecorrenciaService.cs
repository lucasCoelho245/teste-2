using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Application.Interfaces
{
    public interface IInserirAutorizacaoRecorrenciaService
    {
        Task<MensagemPadraoResponse> Handle(InserirAutorizacaoRecorrenciaCommand request);
    }
}