using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Application.Query.QRCode.Traducao;
using System.Net;
using Pay.Recorrencia.Gestao.Api.Filters;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/pix-automatico/qrcode")]
    public class QRCodeController : ControllerBase
    {
        private IMediator Mediator { get; }
        public QRCodeController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost("traduzir")]
        [SwaggerOperation(Summary = "Traduz um QRCode")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<TraducaoQRCodeRecorrencia>))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        [RequiredHeaders]
        public async Task<ActionResult> TraduzirQRCode([FromBody] TraducaoQRCodeRequest request)
        {
            try
            {
                var response = await Mediator.Send(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    StatusCode = 500,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Error = new Error
                    {
                        StatusCode = 500,
                        Message = ex.Message
                    }
                });
            }
        }
    }
}
