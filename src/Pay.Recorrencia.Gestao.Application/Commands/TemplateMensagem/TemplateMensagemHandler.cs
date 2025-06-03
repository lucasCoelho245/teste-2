using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Commands.TemplateMensagem
{
    
public class TemplateMensagemHandler : IRequestHandler<ConsultaTemplateMensagemCommand, TemplateMensagemResponse>
                                       , IRequestHandler<IncluirTemplateMensagemCommand, MensagemPadraoResponse>
    {
        private ITemplateMensagemRepository _templateMensagemRepository;
        private IMapper _mapper { get; }
        public TemplateMensagemHandler(ITemplateMensagemRepository templateMensagemRepository, IMapper mapper)
        {
            _templateMensagemRepository = templateMensagemRepository;
            _mapper = mapper;
        }

        public async Task<TemplateMensagemResponse> Handle(ConsultaTemplateMensagemCommand request, CancellationToken cancellationToken)
        {
            //HIST-085 - Cenário 01
            if (!ValidaCamposObrigatorios(request))
            {
                return await Task.FromResult(new TemplateMensagemResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "ERRO_PIXAUTO-002",
                    Message = "Campos não preenchidos corretamente"
                });
            }

            //HIST-085 - Cenário 02
            TemplateMensagemResponse templateMensagemResponse = new()
            {
                Data = await _templateMensagemRepository.GetTemplateMensagem(idMensagem: request.IdMensagem)
            };

            //HIST-085 - Cenário 03
            if (templateMensagemResponse.Data == null)
            {
                templateMensagemResponse.StatusCode = StatusCodes.Status404NotFound;
                templateMensagemResponse.Status = "ERRO_PIXAUTO-024";
                templateMensagemResponse.Message = "Registro de Mensagem não encontrado";
                return await Task.FromResult(templateMensagemResponse);
            }
            else
            {
                templateMensagemResponse.StatusCode = StatusCodes.Status200OK;
                templateMensagemResponse.Status = "OK";
            }

                return await Task.FromResult(templateMensagemResponse);
        }
             
        private static bool ValidaCamposObrigatorios(ConsultaTemplateMensagemCommand request)
        {
            if (string.IsNullOrEmpty(request.IdMensagem))
            {
                return false;
            }
            return true;
        }

        public async Task<MensagemPadraoResponse> Handle(IncluirTemplateMensagemCommand request, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<Pay.Recorrencia.Gestao.Domain.Entities.TemplateMensagem>(request);
            var templateMensagem = await _templateMensagemRepository.GetTemplateMensagem(dto.idMensagem);
            if (templateMensagem is null)
                await _templateMensagemRepository.Insert(dto);
            else
                return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-023", "Não foi possível realizar a alteração."));

            return await Task.FromResult(new MensagemPadraoResponse(StatusCodes.Status200OK, string.Empty, string.Empty));
        }
    }
}
