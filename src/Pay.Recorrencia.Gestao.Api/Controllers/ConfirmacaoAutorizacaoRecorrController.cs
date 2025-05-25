using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Commands.ConfirmacaoAutorizacaoRecorr;
using Pay.Recorrencia.Gestao.Application.Response;
using Swashbuckle.AspNetCore.Annotations;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/pix-automatico/confirmacao-autorizacoes-recorr")]
    [Produces("application/json")]
    public class ConfirmacaoAutorizacaoRecorrController : ControllerBase
    {
        private IMediator Mediator { get; }

        public ConfirmacaoAutorizacaoRecorrController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost()]
        [SwaggerOperation(Summary = "Recebe a Confirmação de Autorização Recorrência", Description = "Recebe a Confirmação de Autorização Recorrência")]
        [SwaggerResponse(200, Type = typeof(MensagemPadraoResponse))]
        [SwaggerResponse(400)]
        public async Task<ActionResult> RecebeConfirmacaoAutorizacaoRecorr(ReceberConfirmacaoAutorizacaoRecorrCommand command)
        {
            try
            {
                var retornoValidacao = await Mediator.Send(new ValidarConfirmacaoCommand(command));
                if (retornoValidacao.StatusCode == StatusCodes.Status200OK)
                {
                    var response = await Mediator.Send(command);
                    if (response is null || response.StatusCode != StatusCodes.Status200OK)
                    {
                        return BadRequest(response?.Error.Code + "-" + response?.Error.Message);
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest(retornoValidacao.Error.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
