using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Lista;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/pix-automatico")]
    public class SolicAutorizacaoRecController : ControllerBase
    {
        private IMediator Mediator { get; }
        public SolicAutorizacaoRecController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost("solicitacoes")]
        [SwaggerOperation(Summary = "Retorna solicitações")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<SolicAutorizacaoRecList>))]
        [SwaggerResponse(404)]
        [Produces("application/json")]
        public async Task<ActionResult> GetSolicitacoes(GetListaSolicAutorizacaoRecDTO body, [FromQuery] PaginacaoDTO pagination)
        {
            var request = new ListaSolicAutorizacaoRecRequest()
            {
                CpfCnpjUsuarioPagador = body.CpfCnpjUsuarioPagador,
                SituacaoSolicRecorrencia = body.SituacaoSolicRecorrencia,
                NomeUsuarioRecebedor = body.NomeUsuarioRecebedor,
                AgenciaUsuarioPagador = body.AgenciaUsuarioPagador,
                ContaUsuarioPagador = body.ContaUsuarioPagador,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };
            var response = await Mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("solicitacoes/{idSolicitacao}")]
        [SwaggerOperation(Summary = "Retorna todas as informações de uma solicitação")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<SolicitacaoRecorrencia>))]
        [SwaggerResponse(404)]
        [Produces("application/json")]
        public async Task<ActionResult> GetDetalheSolicitacao(string idSolicitacao, [FromQuery] PaginacaoDTO pagination)
        {
            var request = new DetalhesSolicAutorizacaoRecRequest()
            {
                IdSolicRecorrencia = idSolicitacao,
            };
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
