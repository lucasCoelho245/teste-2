
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

            if(!dataFinder.Items.Any()) throw new Exception("Nenhuma autorização encontrada para estes parâmetros de busca");

            var response = new ListaAutorizacaoRecResponse()
            {
                Status = "success",
                StatusCode = 200,
                Data = new ItemsData()
                {
                    Items = dataFinder.Items,
                    Pagination = new Pagination()
                    {
                        TotalItems = dataFinder.TotalItems,
                        TotalPages = (int)Math.Ceiling((double)dataFinder.TotalItems / request.PageSize),
                        PageSize = request.PageSize,
                        CurrentPage = request.Page
                    },
                },
                Message = "Operação realizada com sucesso"
            };
            return await Task.FromResult(response);
        }
    }
}
