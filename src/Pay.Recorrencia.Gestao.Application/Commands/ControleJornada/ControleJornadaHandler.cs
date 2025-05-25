using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Commands.ControleJornada
{
    public class ControleJornadaHandler : IRequestHandler<IncluirControleJornadaCommand, MensagemPadraoResponse>,
                                            IRequestHandler<AtualizarControleJornadaCommand, MensagemPadraoResponse>
    {
        private IControleJornadaRepository _controleJornadaRepository { get; }
        private IMapper _mapper { get; }

        public ControleJornadaHandler(IControleJornadaRepository controleJornadaRepository, IMapper mapper)
        {
            _controleJornadaRepository = controleJornadaRepository;
            _mapper = mapper;
        }

        public async Task<MensagemPadraoResponse> Handle(IncluirControleJornadaCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<Domain.Entities.ControleJornada>(request);

            try
            {
                await _controleJornadaRepository.IncluirControle(dto);
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, string.Empty));
            }
            catch (Exception)
            {
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-021", "Controle de jornada já existente."));
            }
        }

        public async Task<MensagemPadraoResponse> Handle(AtualizarControleJornadaCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<Domain.Entities.ControleJornada>(request);

            try
            {
                await _controleJornadaRepository.AtualizarControle(dto);
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, string.Empty));
            }
            catch (Exception)
            {
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-020", "Controle de jornada não encontrada."));
            }
        }
    }
}
