using AutoMapper;
using MediatR;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Detalhes
{
    public class DetalhesControleJornadaHandler : IRequestHandler<DetalhesControleJornadaAutorizacaoResquest, DetalhesControleJornadaResponse>,
                                                    IRequestHandler<DetalhesControleJornadaAgendamentoResquest, DetalhesControleJornadaResponse>

    {
        private IJornadaRepository _jornadaRepository { get; }
        private IMapper _mapper { get; }

        public DetalhesControleJornadaHandler(IJornadaRepository jornadaRepository, IMapper mapper)
        {
            _jornadaRepository = jornadaRepository;
            _mapper = mapper;
        }

        public async Task<DetalhesControleJornadaResponse> Handle(DetalhesControleJornadaAutorizacaoResquest request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<Domain.DTO.JornadaAutorizacaoDTO>(request);

            var dataFinder = await _jornadaRepository.GetByTpJornadaAndIdRecorrenciaAsync(dto);

            if (dataFinder.Data is null) throw new Exception("Nenhuma solicitacao encontrada para estes parâmetros de busca");

            bool statusBool = dataFinder.Data != null;
            string status = statusBool ? "OK" : "";

            var response = new DetalhesControleJornadaResponse()
            {
                Status = status,
                StatusCode = statusBool ? 200 : 404,
                Data = dataFinder.Data,
                Message = statusBool ? "" : "Nenhuma jornada encontrada para estes parâmetros de busca"
            };
            return await Task.FromResult(response);
        }


        public async Task<DetalhesControleJornadaResponse> Handle(DetalhesControleJornadaAgendamentoResquest request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<Domain.DTO.JornadaAgendamentoDTO>(request);

            var dataFinder = await _jornadaRepository.GetByTpJornadaAndIdE2EAsync(dto);

            if (dataFinder.Data is null) throw new Exception("Nenhuma solicitacao encontrada para estes parâmetros de busca");

            bool statusBool = dataFinder.Data != null;
            string status = statusBool ? "OK" : "";

            var response = new DetalhesControleJornadaResponse()
            {
                Status = status,
                StatusCode = statusBool ? 200 : 404,
                Data = dataFinder.Data,
                Message = statusBool ? "" : "Nenhuma jornada encontrada para estes parâmetros de busca"
            };
            return await Task.FromResult(response);
        }
    }
}
