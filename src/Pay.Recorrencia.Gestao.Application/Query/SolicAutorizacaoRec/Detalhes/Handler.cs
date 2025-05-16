using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes
{
    public class DetalhesSolicAutorizacaoRecHandler : IRequestHandler<DetalhesSolicAutorizacaoRecRequest, DetalhesSolicAutorizacaoRecResponse>
    {
        private ISolicitacaoRecorrenciaRepository _repository { get; }

        public DetalhesSolicAutorizacaoRecHandler(
            ISolicitacaoRecorrenciaRepository repository)
        {
            _repository = repository;
        }

        public async Task<DetalhesSolicAutorizacaoRecResponse> Handle(DetalhesSolicAutorizacaoRecRequest request, CancellationToken cancellationToken)
        {
            var dataFinder = await _repository.GetAsync(request);

            bool status = dataFinder.Items.Any();

            var response = new DetalhesSolicAutorizacaoRecResponse()
            {
                Status = status,
                StatusCode = status ? 200 : 404,
                Data = new ItemsData()
                {
                    Items = dataFinder.Items,
                },
                Pagination = new Pagination()
                {
                    TotalItems = dataFinder.TotalItems,
                    TotalPages = (int)Math.Ceiling((double)dataFinder.TotalItems / request.PageSize),
                    PageSize = request.PageSize,
                    CurrentPage = request.Page
                },
                Message = status ? "" : "ERRO-PIXAUTO-003"
            };

            return await Task.FromResult(response);
        }
    }
}
