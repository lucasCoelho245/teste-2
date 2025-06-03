using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Commands.ConsultaAgendamentoCobranca;
using Pay.Recorrencia.Gestao.Application.Commands.ConsultaDetalheDadosCobranca;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/pix-automatico/consulta-cobranca")]

    public class ConsultaCobrancaController(IMediator mediator) : Controller
    {
        [HttpPost("detalhe")]
        [SwaggerOperation(Summary = "Consulta detalhes dos dados de uma cobrança", Description = "Obtém dados de uma cobrança de Pix Automático agendada")]
        [SwaggerResponse(200, Type = typeof(DetalheDadosCobranca))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> ConsultaDetalheDadosCobranca(ConsultaDetalheDadosCobrancaCommand command)
        {
            try
            {
                var detalhes = await mediator.Send(command);

                if (detalhes == null)
                {
                    return NotFound();
                }

                return Ok(detalhes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new MensagemPadraoResponse(StatusCodes.Status400BadRequest, ex.Message, "Campos obrigatorios preenchidos de forma incorreta ou vazios"));
            }
            catch (Exception)
            {
                return StatusCode(500, new MensagemPadraoResponse(StatusCodes.Status500InternalServerError ,"","Erro interno do servidor"));
            }
        }

        [HttpPost("agendamento")]
        [SwaggerOperation(Summary = "Consulta os pagamentos agendados", Description = "Obtém uma lista de Pix Automático agendados")]
        [SwaggerResponse(200, Type = typeof(PixAgendamentoDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> ConsultaAgendamentoCobranca(ConsultaAgendamentoCobrancaCommand command)
        {
            try
            {
                var detalhes = await mediator.Send(command);

                if (!detalhes.Any())
                {
                    return NotFound();
                }

                return Ok(detalhes);
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
    }
}
