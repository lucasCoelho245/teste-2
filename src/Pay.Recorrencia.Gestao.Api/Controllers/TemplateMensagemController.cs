using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Commands.TemplateMensagem;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/pix-automatico/template-mensagem")]
    public class TemplateMensagemController : ControllerBase
    {
        private IMediator _mediator { get; }

        public TemplateMensagemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-template-mensagem/{idMensagem}")]
        [SwaggerOperation(Summary = "Consulta template de mensagens", Description = "Obtém dados de uma cobrança de Pix Automático agendada")]
        [SwaggerResponse(200, Type = typeof(TemplateMensagemResponse))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> GetTemplateMensagem(string idMensagem)
        {
            try
            {
                var request = new ConsultaTemplateMensagemCommand { IdMensagem = idMensagem };

                var template = await _mediator.Send(request);

                if (template == null)
                {
                    return NotFound(new MensagemPadraoResponse(StatusCodes.Status404NotFound, "ERRO-PIXAUTO-024", "Registro de Mensagem não encontrado"));
                }

                return Ok(template);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, ex.Message, "Campos obrigatorios preenchidos de forma incorreta ou vazios"));
            }
            catch (Exception)
            {
                return StatusCode(500, new MensagemPadraoResponse(StatusCodes.Status500InternalServerError, "", "Erro interno do servidor"));
            }
        }

        [HttpPost("insercao-template-mensagem")]
        [SwaggerOperation(Summary = "Inserção de Template", Description = "Inserção de novo Template")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MensagemPadraoResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(MensagemPadraoResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(MensagemPadraoResponse))]
        public async Task<IActionResult> InsertTemplateAsync([FromBody] IncluirTemplateMensagemCommand request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-002", "Campos não preenchidos corretamente."));
                
                var response = await _mediator.Send(request);

                if (response.StatusCode != StatusCodes.Status200OK)
                    return StatusCode(response.StatusCode, response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MensagemPadraoResponse(StatusCodes.Status500InternalServerError, string.Empty, ex.Message.ToString()));
            }
        }
    }
}
