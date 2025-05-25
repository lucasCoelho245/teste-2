using AutoMapper;
using Microsoft.AspNetCore.Http;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Interfaces;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Services
{
    public class AtualizarAutorizacaoRecorrenciaService : IAtualizarAutorizacaoRecorrenciaService
    {
        private ISolicitacaoRecorrenciaRepository _solicitacaoRecorrenciaRepository { get; }
        private IMapper _mapper { get; }

        public AtualizarAutorizacaoRecorrenciaService(ISolicitacaoRecorrenciaRepository solicitacaoRecorrenciaRepository, IMapper mapper)
        {
            _solicitacaoRecorrenciaRepository = solicitacaoRecorrenciaRepository;
            _mapper = mapper;
        }

        public async Task<MensagemPadraoResponse> Handle(AtualizarSolicitacaoRecorrenciaCommand request)
        {
            var dto = _mapper.Map<SolicitacaoAutorizacaoRecorrenciaUpdateDTO>(request);
            var solicitacaoRecorrencia = await _solicitacaoRecorrenciaRepository.GetSolicitacaoRecorrencia(dto.IdSolicRecorrencia);
            if (solicitacaoRecorrencia != null)
                await _solicitacaoRecorrenciaRepository.Update(dto);
            else
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-005", "Chave não encontrada na tabela SOLICITACAO_AUTORIZACAO_RECORRENCIA"));

            return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, "OK"));
        }
    }
}
