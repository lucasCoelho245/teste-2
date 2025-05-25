using MediatR;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Commands.IncluirInformacaoSolicitacao
{
    public class IncluirInformacaoSolicitacaoHandler : IRequestHandler<IncluirInformacaoSolicitacaoRequest, long>
    {
        private IInformacaoSolicitacaoRepository _informacaoSolicitacaoRepository;


        public IncluirInformacaoSolicitacaoHandler(IInformacaoSolicitacaoRepository informacaoSolicitacaoRepository)
        {
            _informacaoSolicitacaoRepository = informacaoSolicitacaoRepository;
        }

        public async Task<long> Handle(IncluirInformacaoSolicitacaoRequest request, CancellationToken cancellationToken)
        {
            var novoSequencial = await _informacaoSolicitacaoRepository.ObterENovoSequencialAsync(request.Ispb);

            return await Task.FromResult(novoSequencial);
        }
    }
}