using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Api.Filters;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Lista;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Application.Commands.AprovarSolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/pix-automatico/solicitacao-autorizacao-recorrencia")]
    public class SolicitacaoAutorizacaoRecorrenciaController : ControllerBase
    {
        //private readonly ILogger<SolicitacaoRecorrencia> _logger;
        private IMediator _mediator { get; }

        public SolicitacaoAutorizacaoRecorrenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MensagemPadraoResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MensagemPadraoResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(MensagemPadraoResponse))]
        public async Task<IActionResult> CriarSolicitacaoRecorrenciaAsync([FromBody] IncluirSolicitacaoRecorrenciaCommand request)
        {
            MensagemPadraoResponse response;
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new MensagemPadraoResponse(StatusCodes.Status400BadRequest, string.Empty, "Erro de validação nos dados fornecidos."));

                response = await _mediator.Send(request);

                if (response.StatusCode != StatusCodes.Status200OK)
                    return StatusCode(response.StatusCode, response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MensagemPadraoResponse(StatusCodes.Status500InternalServerError, string.Empty, ex.Message.ToString()));
                //return StatusCode(StatusCodes.Status500InternalServerError, new MensagemPadraoResponse(StatusCodes.Status500InternalServerError, string.Empty, "Erro interno no servidor."));
            }
        }

        [HttpPut("{idSolicRecorrencia}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MensagemPadraoResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MensagemPadraoResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(MensagemPadraoResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(MensagemPadraoResponse))]
        public async Task<IActionResult> AtualizarSolicitacaoRecorrenciaAsync(string idSolicRecorrencia, [FromBody] AtualizarSolicitacaoRecorrenciaCommand request)
        {
            MensagemPadraoResponse response;

            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new MensagemPadraoResponse(StatusCodes.Status400BadRequest, string.Empty, "Erro de validação nos dados fornecidos."));

                request.IdSolicRecorrencia = idSolicRecorrencia;

                response = await _mediator.Send(request);

                if (response.StatusCode != StatusCodes.Status200OK)
                    return StatusCode(response.StatusCode, response);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MensagemPadraoResponse(StatusCodes.Status500InternalServerError, string.Empty, "Erro interno no servidor."));
            }
        }

        [HttpGet("lista")]
        [SwaggerOperation(Summary = "Retorna solicitações")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<SolicAutorizacaoRecList>))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        [RequiredHeaders]
        public async Task<ActionResult> GetSolicitacoes([FromQuery] GetListaSolicAutorizacaoRecDTO data, [FromQuery] PaginacaoDTO pagination)
        {
            var request = new ListaSolicAutorizacaoRecRequest()
            {
                CpfCnpjUsuarioPagador = data.CpfCnpjUsuarioPagador,
                SituacaoSolicRecorrencia = data.SituacaoSolicRecorrencia,
                NomeUsuarioRecebedor = data.NomeUsuarioRecebedor,
                AgenciaUsuarioPagador = data.AgenciaUsuarioPagador,
                ContaUsuarioPagador = data.ContaUsuarioPagador,
                DtExpiracaoInicio = data.DtExpiracaoInicio,
                DtExpiracaoFim = data.DtExpiracaoFim,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };
            try
            {
                var response = await _mediator.Send(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new ErrorResponse
                {
                    StatusCode = 404,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Error = new Error
                    {
                        StatusCode = 404,
                        Message = ex.Message
                    }
                });
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todas as informações de uma solicitação")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataNonPaginatedResponse<SolicitacaoRecorrencia>))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        [RequiredHeaders]
        public async Task<ActionResult> GetDetalheSolicitacao([FromQuery] GetSolicAutorizacaoRecDTO data)
        {
            var request = new DetalhesSolicAutorizacaoRecRequest()
            {
                IdSolicRecorrencia = data.IdSolicRecorrencia,
                IdRecorrencia = data.IdRecorrencia
            };
            try
            {
                var response = await _mediator.Send(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new ErrorResponse
                {
                    StatusCode = 404,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Error = new Error
                    {
                        StatusCode = 404,
                        Message = ex.Message
                    }
                });
            }
        }


        [HttpPost("aprovar")]
        [SwaggerOperation(Summary = "Aprova a Autorização Recorrência", Description = "Aprova uma Autorização Recorrência.")]
        [SwaggerResponse(200, Type = typeof(AutorizacaoRecorrencia))]
        [SwaggerResponse(400)]
        public async Task<ActionResult> AprovarSolicitacaoAutorizacaoRecorrencia(AprovarSolicitacaoRecorrenciaCommand request)
        {
            MensagemPadraoResponse response;
            try
            {

                response = await _mediator.Send(request);
                //Logger.Information("Response: {@Response}", response);
                if (response is null)
                {
                    //Logger.Warning("Autorização Recorrência não gravada.");
                    return BadRequest();
                }

                if (!response.StatusCode.Equals(StatusCodes.Status200OK))
                {
                    return BadRequest(response);
                }

                //Logger.Information("Autorização Recorrência gravada com sucesso. {@Response}", response);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
