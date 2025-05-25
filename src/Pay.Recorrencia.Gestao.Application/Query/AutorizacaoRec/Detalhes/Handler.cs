using Confluent.Kafka;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Detalhes
{
    public class DetalhesAutorizacaoRecHandler : IRequestHandler<DetalhesAutorizacaoRecRequest, DetalhesAutorizacaoRecResponse>
    {
        private IMockAutorizacaoRecorrenciaRepository _repository { get; }

        public DetalhesAutorizacaoRecHandler(
            IMockAutorizacaoRecorrenciaRepository repository)
        {
            _repository = repository; 
        }

        public async Task<DetalhesAutorizacaoRecResponse> Handle(DetalhesAutorizacaoRecRequest request, CancellationToken cancellationToken)
        {
            var dataFinder = await _repository.GetAsync(request);

            if(dataFinder.Data == null) throw new Exception("Nenhuma autorização encontrada para estes parâmetros de busca");

            var response = new DetalhesAutorizacaoRecResponse()
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
