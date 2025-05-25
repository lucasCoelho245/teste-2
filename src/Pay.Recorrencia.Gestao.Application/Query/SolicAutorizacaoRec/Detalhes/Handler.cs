using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes
{
    public class DetalhesSolicAutorizacaoRecHandler : IRequestHandler<DetalhesSolicAutorizacaoRecRequest, DetalhesSolicAutorizacaoRecResponse>
    {
        private IMockSolicitacaoRecorrenciaRepository _repository { get; }

        public DetalhesSolicAutorizacaoRecHandler(
            IMockSolicitacaoRecorrenciaRepository repository)
        {
            _repository = repository;
        }

        public async Task<DetalhesSolicAutorizacaoRecResponse> Handle(DetalhesSolicAutorizacaoRecRequest request, CancellationToken cancellationToken)
        {
            var dataFinder = await _repository.GetAsync(request);

            if(dataFinder.Data == null) throw new Exception("Nenhuma solicitacao encontrada para estes parâmetros de busca");

            var response = new DetalhesSolicAutorizacaoRecResponse()
            {
                Status = "OK",
                StatusCode = 200,
                Data = dataFinder.Data,
                Message = "Operação realizada com sucesso"
            };

            return await Task.FromResult(response);
        }
    }
}
