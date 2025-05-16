using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [Route("solicitacao-recorrencia")]
    public class SolicitacaoRecorrenciaController : ControllerBase
    {
        //private readonly ILogger<SolicitacaoRecorrencia> _logger;
        private IMediator _mediator { get; }

        public SolicitacaoRecorrenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarSolicitacaoRecorrenciaAsync([FromBody] IncluirSolicitacaoRecorrenciaCommand request)
        {
            MensagemPadraoResponse response;
            try
            {
                if (!ModelState.IsValid)
                    return CreateResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados fornecidos.");

                response = await _mediator.Send(request);

                return Ok(response);
            }
            catch
            {
                return CreateResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor.");
            }
        }

        [HttpPut("{idSolicRecorrencia}")]
        [ProducesResponseType(typeof(MensagemPadraoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarSolicitacaoRecorrenciaAsync(string idSolicRecorrencia, [FromBody] AtualizarSolicitacaoRecorrenciaCommand request)
        {
            MensagemPadraoResponse response;

            try
            {
                if (!ModelState.IsValid)
                    return CreateResponse(StatusCodes.Status400BadRequest, "Erro de validação nos dados fornecidos.");

                request.IdSolicRecorrencia = idSolicRecorrencia;

                response = await _mediator.Send(request);

                if (response.CodigoRetorno != StatusCodes.Status200OK.ToString())
                    return CreateResponse(StatusCodes.Status404NotFound, response.CodigoRetorno, response.MensagemErro);

                return Ok(response);
            }
            catch
            {
                return CreateResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor.");
            }
        }

        private ObjectResult CreateResponse(int statusCode, string mensagemErro, MensagemPadraoResponse? data = null)
        {
            var response = new MensagemPadraoResponse
            {
                CodigoRetorno = statusCode.ToString(),
                MensagemErro = mensagemErro
            };

            // Retorna o objeto com o status code apropriado
            return StatusCode(statusCode, data ?? response);
        }
        private ObjectResult CreateResponse(int statusCode, string codigoRetorno, string mensagemErro, MensagemPadraoResponse? data = null)
        {
            var response = new MensagemPadraoResponse
            {
                CodigoRetorno = codigoRetorno,
                MensagemErro = mensagemErro
            };

            // Retorna o objeto com o status code apropriado
            return StatusCode(statusCode, data ?? response);
        }
    }
}
