
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Lista
{
    public class ListaAutorizacaoRecHandler : IRequestHandler<ListaAutorizacaoRecRequest, ListaAutorizacaoRecResponse>
    {
        private IAutorizacaoRecorrenciaRepository _repository { get; }

        public ListaAutorizacaoRecHandler(
            IAutorizacaoRecorrenciaRepository repository)
        {
            _repository = repository; 
        }

        public async Task<ListaAutorizacaoRecResponse> Handle(ListaAutorizacaoRecRequest request, CancellationToken cancellationToken)
        {
            var dataFinder = await _repository.GetAllAsync(request);

            bool status = dataFinder.Items.Any();
            
            var response = new ListaAutorizacaoRecResponse()
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
                    PageSize  = request.PageSize,
                    CurrentPage = request.Page
                },
                Message = status ? "" : "Nenhuma autorização encontrada para estes parâmetros de busca"
            };
            return await Task.FromResult(response);
        }
    }
}
