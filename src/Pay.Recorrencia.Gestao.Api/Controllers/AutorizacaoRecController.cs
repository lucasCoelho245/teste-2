using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Lista;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Detalhes;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/pix-automatico")]
    public class AutorizacaoRecController : ControllerBase
    {
        private IMediator Mediator { get; }
        public AutorizacaoRecController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost("autorizacoes")]
        [SwaggerOperation(Summary = "Retorna autorizações")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<AutorizacaoRecList>))]
        [SwaggerResponse(404)]
        [Produces("application/json")]
        public async Task<ActionResult> GetAutorizacoes(GetListaAutorizacaoRecDTO body, [FromQuery] PaginacaoDTO pagination)
        {
            var request = new ListaAutorizacaoRecRequest()
            {
                CpfCnpjUsuarioPagador = body.CpfCnpjUsuarioPagador,
                SituacaoRecorrencia = body.SituacaoRecorrencia,
                NomeUsuarioRecebedor = body.NomeUsuarioRecebedor,
                AgenciaUsuarioPagador = body.AgenciaUsuarioPagador,
                ContaUsuarioPagador = body.ContaUsuarioPagador,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };
            var response = await Mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("autorizacoes/{idAutorizacao}/recorrencia/{idRecorrencia}")]
        [SwaggerOperation(Summary = "Retorna todas as informações de uma solicitação")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<AutorizacaoRecorrencia>))]
        [SwaggerResponse(404)]
        [Produces("application/json")]
        public async Task<ActionResult> GetDetalheAutorizacao(Guid idAutorizacao, Guid idRecorrencia, [FromQuery] PaginacaoDTO pagination)
        {
            var request = new DetalhesAutorizacaoRecRequest()
            {
                IdAutorizacao = idAutorizacao,
                IdRecorrencia = idRecorrencia
            };
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
