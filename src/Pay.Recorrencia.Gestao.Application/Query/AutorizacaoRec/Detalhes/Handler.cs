using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Detalhes
{
    public class DetalhesAutorizacaoRecHandler : IRequestHandler<DetalhesAutorizacaoRecRequest, DetalhesAutorizacaoRecResponse>
    {
        private IAutorizacaoRecorrenciaRepository _repository { get; }

        public DetalhesAutorizacaoRecHandler(
            IAutorizacaoRecorrenciaRepository repository)
        {
            _repository = repository; 
        }

        public async Task<DetalhesAutorizacaoRecResponse> Handle(DetalhesAutorizacaoRecRequest request, CancellationToken cancellationToken)
        {
            var dataFinder = await _repository.GetAsync(request);

            bool status = dataFinder.Items.Any();
            
            var response = new DetalhesAutorizacaoRecResponse()
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
                Message = status ? "" : "ERRO-PIXAUTO-003"
            };

            return await Task.FromResult(response);
        }
    }
}
