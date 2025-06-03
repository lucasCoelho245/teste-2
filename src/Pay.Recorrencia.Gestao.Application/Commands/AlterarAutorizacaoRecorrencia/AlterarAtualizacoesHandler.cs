using MediatR;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia
{
    public sealed class AlterarAtualizacoesHandler : IRequestHandler<AlterarAutorizacaoCommand, MensagemPadraoResponse>
    {
        //private ILogger Logger { get; }
        private IAtualizarAutorizacaoService _atualizarAutorizacaoService { get; }

        //public UpdateAtualizacoesHandler(ILogger logger, IAutorizacaoRecorrenciaRepository atualizacoesRepository)
        public AlterarAtualizacoesHandler(IAtualizarAutorizacaoService atuaizarAutorizacaoService)
        {
            //Logger = logger;
            _atualizarAutorizacaoService = atuaizarAutorizacaoService;
        }

        public async Task<MensagemPadraoResponse> Handle(AlterarAutorizacaoCommand request, CancellationToken cancellationToken)
        {
            return await _atualizarAutorizacaoService.Handle(request);
        }
    }
}
