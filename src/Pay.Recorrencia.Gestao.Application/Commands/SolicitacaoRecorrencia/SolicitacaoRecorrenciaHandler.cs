using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia
{
    public class SolicitacaoRecorrenciaHandler : IRequestHandler<IncluirSolicitacaoRecorrenciaCommand, MensagemPadraoResponse>,
                                                 IRequestHandler<AtualizarSolicitacaoRecorrenciaCommand, MensagemPadraoResponse>
    {

        private ISolicitacaoRecorrenciaRepository _solicitacaoRecorrenciaRepository { get; }
        private IMapper _mapper { get; }

        public SolicitacaoRecorrenciaHandler(ISolicitacaoRecorrenciaRepository solicitacaoRecorrenciaRepository, IMapper mapper)
        {
            _solicitacaoRecorrenciaRepository = solicitacaoRecorrenciaRepository;
            _mapper = mapper;
        }

        public async Task<MensagemPadraoResponse> Handle(IncluirSolicitacaoRecorrenciaCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<Domain.Entities.SolicitacaoRecorrencia>(request);
            var solicitacaoRecorrencia = await _solicitacaoRecorrenciaRepository.GetSolicitacaoRecorrencia(dto.IdSolicRecorrencia);
            if (solicitacaoRecorrencia == null)
                await _solicitacaoRecorrenciaRepository.Insert(dto);
            else
                return await Task.FromResult(new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-001", MensagemErro = "Solicitação já existente." });

            return await Task.FromResult(new MensagemPadraoResponse() { CodigoRetorno = StatusCodes.Status200OK.ToString(), MensagemErro = "Solicitação criada com sucesso." });
        }

        public async Task<MensagemPadraoResponse> Handle(AtualizarSolicitacaoRecorrenciaCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<SolicitacaoAutorizacaoRecorrenciaUpdateDTO>(request);
            var solicitacaoRecorrencia = await _solicitacaoRecorrenciaRepository.GetSolicitacaoRecorrencia(dto.IdSolicRecorrencia);
            if (solicitacaoRecorrencia != null)
                await _solicitacaoRecorrenciaRepository.Update(dto);
            else
                return await Task.FromResult(new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-005", MensagemErro = "Solicitação não encontrada." });

            return await Task.FromResult(new MensagemPadraoResponse() { CodigoRetorno = StatusCodes.Status200OK.ToString(), MensagemErro = "Solicitação atualizada com sucesso." });
        }
    }
}
