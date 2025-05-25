using MediatR;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia
{
    public class InserirAutorizacaoRecorrenciaHandler : IRequestHandler<InserirAutorizacaoRecorrenciaCommand, MensagemPadraoResponse>
    {
        //private ILogger Logger { get; }
        private IInserirAutorizacaoRecorrenciaService _inserirAutorizacaoRecorrenciaService;
        //public InserirAutorizacaoRecorrenciaHandler(IAutorizacaoRecorrenciaRepository autorizacaoRecorrenciaRepository, ILogger logger)

        public InserirAutorizacaoRecorrenciaHandler(IInserirAutorizacaoRecorrenciaService inserirAutorizacaoRecorrenciaService)
        {
            _inserirAutorizacaoRecorrenciaService = inserirAutorizacaoRecorrenciaService;
            //Logger = logger;
        }

        public async Task<MensagemPadraoResponse> Handle(InserirAutorizacaoRecorrenciaCommand request, CancellationToken cancellationToken)
        {
            return await _inserirAutorizacaoRecorrenciaService.Handle(request);
        }
    }
}
