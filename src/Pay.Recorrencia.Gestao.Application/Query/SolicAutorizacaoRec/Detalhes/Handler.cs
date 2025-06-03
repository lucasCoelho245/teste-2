using MediatR;
using AutoMapper;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes
{
    public class DetalhesSolicAutorizacaoRecHandler : IRequestHandler<DetalhesSolicAutorizacaoRecRequest, DetalhesSolicAutorizacaoRecResponse>
    {
        private ISolicitacaoRecorrenciaRepository _repository { get; }
        private readonly IMapper _mapper;

        public DetalhesSolicAutorizacaoRecHandler(
            ISolicitacaoRecorrenciaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DetalhesSolicAutorizacaoRecResponse> Handle(DetalhesSolicAutorizacaoRecRequest request, CancellationToken cancellationToken)
        {
            var dataFinder = await _repository.GetAsync(request);

            if(dataFinder.Data == null) throw new Exception("Nenhuma solicitacao encontrada para estes parâmetros de busca");

            var data = _mapper.Map<SolicitacaoAutorizacaoRecorrenciaDetalhesDTO>(dataFinder.Data);
            var response = new DetalhesSolicAutorizacaoRecResponse()
            {
                Status = "success",
                StatusCode = 200,
                Data = data,
                Message = "Operação realizada com sucesso"
            };

            return await Task.FromResult(response);
        }
    }
}
