
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Lista
{
    public class ListaSolicAutorizacaoRecHandler : IRequestHandler<ListaSolicAutorizacaoRecRequest, ListaSolicAutorizacaoRecResponse>
    {
        private ISolicitacaoRecorrenciaRepository _repository { get; }

        public ListaSolicAutorizacaoRecHandler(
            ISolicitacaoRecorrenciaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ListaSolicAutorizacaoRecResponse> Handle(ListaSolicAutorizacaoRecRequest request, CancellationToken cancellationToken)
        {
            var dataFinder = await _repository.GetAllAsync(request);

            bool status = dataFinder.Items.Any();

            var response = new ListaSolicAutorizacaoRecResponse()
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
                Message = status ? "" : "Nenhuma solicitação encontrada para estes parâmetros de busca"
            };
            return await Task.FromResult(response);
        }
    }
}
