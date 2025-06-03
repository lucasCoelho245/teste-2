using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Api.Filters;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrenciaCanal;
using Pay.Recorrencia.Gestao.Application.Commands.AprovarRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.CancelarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Lista;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/pix-automatico/autorizacoes-recorrencia")]
    [Produces("application/json")]
    public class AutorizacaoRecorrenciaController : Controller
    {
        //private ILogger Logger { get; }
        private IMediator Mediator { get; }

        //public AutorizacaoRecorrenciaController(ILogger logger, IMediator mediator)
        public AutorizacaoRecorrenciaController(IMediator mediator)

        {
            Mediator = mediator;
            //Logger = logger;
        }

        [HttpPatch("canal")]
        [SwaggerOperation(Summary = "Altera Autorização de Recorrência (Canal)", Description = "Permite a alteração de informações de uma Autorização de Recorrência iniciada por um usuário no canal.")]
        [SwaggerResponse(200, Type = typeof(ApiSimpleResponse))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AlterarAutorizacaoCanal([FromBody] AlterarAutorizacaoRecorrenciaCanalCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);

                if (response is null)
                {
                    return NotFound(new ApiSimpleResponse("NOK", "Autorização de recorrência não encontrada."));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiSimpleResponse("NOK", ex.Message));
            }
        }

        [HttpPost()]
        [SwaggerOperation(Summary = "Insere a Autorização Recorrência", Description = "Insere uma nova Autorização Recorrência.")]
        [SwaggerResponse(200, Type = typeof(AutorizacaoRecorrencia))]
        [SwaggerResponse(400)]
        public async Task<ActionResult> InsereDadosAutorizacaoRecorrencia(InserirAutorizacaoRecorrenciaCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
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

        [HttpPost("aprovar")]
        [SwaggerOperation(Summary = "Aprova a Autorização Recorrência", Description = "Aprova uma Autorização Recorrência.")]
        [SwaggerResponse(200, Type = typeof(AutorizacaoRecorrencia))]
        [SwaggerResponse(400)]
        public async Task<ActionResult> AprovarAutorizacaoRecorrencia(AprovarRecorrenciaCommand command)
        {

            try
            {
                var response = await Mediator.Send(command);
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

        [HttpPut]
        [SwaggerOperation(Summary = "Atualiza a Autorização Recorrências", Description = "Altera uma Atualizações de Autorizações de Recorrências.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AtualizacaoAutorizacaoRecorrencia>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AtualizaDadosAutorizacaoRecorrencia([FromBody] AlterarAutorizacaoCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);

                if (response is null)
                {
                    //Logger.LogWarning("Recorrência com ID {Id} não encontrada.");
                    return NotFound();
                }

                if (!response.StatusCode.Equals(StatusCodes.Status200OK))
                {
                    return BadRequest(response);
                }

                //Logger.LogInformation("Atualização de Autorização de Recorrência atualizada: {@Response}", response);
                var retorno = new ApiSimpleResponse("OK", string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var retorno = new ApiSimpleResponse("NOK", ex.Message);
                return BadRequest(retorno);
            }
        }
        
        [HttpPost("cancelar")]
        [SwaggerOperation(Summary = "Cancela a Autorização Recorrência", Description = "Cancela a Autorização Recorrência.")]
        [SwaggerResponse(200, Type = typeof(AutorizacaoRecorrencia))]
        [SwaggerResponse(400)]
        public async Task<ActionResult> CancelarSolicitarAutorizacaoRecorrencia(CancelarAutorizacaoRecorrenciaCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
                //Logger.Information("Response: {@Response}", response);
                if (response is null)
                {
                    //Logger.Warning("Autorização Recorrência não foi cancelada.");
                    return BadRequest();
                }

                if (!response.StatusCode.Equals(StatusCodes.Status200OK))
                {
                    return BadRequest(response);
                }

                //Logger.Information("Cancelamento da Recorrência efetuada com sucesso. {@Response}", response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool ValidaInformacoesRecebidas(InserirAutorizacaoRecorrenciaCommand autorizacaoRecorrenciaCommand)
        {
            if (autorizacaoRecorrenciaCommand.IdRecorrencia.ToString() == string.Empty)
                return false;

            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.SituacaoRecorrencia) ||
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PDRC" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PRRC" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "RCSD" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PDCF" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "LIDO" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PDPG" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "CFPG" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "ERPG" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "PRCF" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "CFDB" &&
                 autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "ERFC")
                return false;

            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TipoRecorrencia) ||
                autorizacaoRecorrenciaCommand.TipoRecorrencia != "RCUR")
                return false;

            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TipoFrequencia) ||
                autorizacaoRecorrenciaCommand.TipoFrequencia != "MIAN" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "MNTH" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "QURT" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "WEEK" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "YEAR")
                return false;

            if (autorizacaoRecorrenciaCommand.DataInicialAutorizacaoRecorrencia.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.NomeUsuarioRecebedor.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.CpfCnpjUsuarioRecebedor.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.ParticipanteDoUsuarioRecebedor.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.CpfCnpjUsuarioPagador.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.ContaUsuarioPagador.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.ParticipanteDoUsuarioPagador.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.NumeroContrato.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.SituacaoRecorrencia != "LIDO")
            {
                if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.DataHoraCriacaoRecorr.ToString()))
                    return false;
            }

            if (!autorizacaoRecorrenciaCommand.TipoRecorrencia.ToString().Equals("RCUR"))
                return false;

            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TipoFrequencia) ||
                autorizacaoRecorrenciaCommand.TipoFrequencia != "MIAN" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "MNTH" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "QURT" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "WEEK" &&
                autorizacaoRecorrenciaCommand.TipoFrequencia != "YEAR")
            {
                return false;
            }


            if (!String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.CodigoMoedaAutorizacaoRecorrencia?.ToString()) &&
                autorizacaoRecorrenciaCommand.CodigoMoedaAutorizacaoRecorrencia != "BRL")
                return false;


            if (String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia) ||
                                     autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "READ"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "RCSD"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "CRTN"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "AUT1"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "AUT2"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "AUT3"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "AUT4"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "CFDB"
                                  && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia != "CCLD")
                return false;

            if (autorizacaoRecorrenciaCommand.TpRetentativa.Equals("NAO_PERMITE") && autorizacaoRecorrenciaCommand.TpRetentativa.Equals("PERMITE_3R_7D"))
                return false;

            if (!String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.MotivoRejeicaoRecorrencia) &&
                autorizacaoRecorrenciaCommand.MotivoRejeicaoRecorrencia != "AP13" &&
                autorizacaoRecorrenciaCommand.MotivoRejeicaoRecorrencia != "AP14")
                return false;

            if (!String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia) &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "ACCL" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "CPCL" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "DCSD" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "ERSL" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "FRUD" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "NRES" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "PCFD" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "SLCR" &&
                autorizacaoRecorrenciaCommand.CodigoSituacaoCancelamentoRecorrencia != "SLDB")
                return false;

            if (!String.IsNullOrEmpty(autorizacaoRecorrenciaCommand.TpRetentativa) &&
                autorizacaoRecorrenciaCommand.TpRetentativa != "NAO_PERMITE" &&
                autorizacaoRecorrenciaCommand.TpRetentativa != "PERMITE_3R_7D")
                return false;

            if (DateTime.Now < autorizacaoRecorrenciaCommand.DataHoraCriacaoRecorr) // ASSUMINDO QUE DATAAUTORIZACAO É O MOMENTO DA CHAMADA À API
                return false;

            if (autorizacaoRecorrenciaCommand.DataCancelamento < autorizacaoRecorrenciaCommand.DataHoraCriacaoRecorr)
                return false;

            return true;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retorna autorizações")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<AutorizacaoRecList>))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        [RequiredHeaders]
        public async Task<ActionResult> GetAutorizacoes([FromQuery] GetListaAutorizacaoRecDTO data, [FromQuery] PaginacaoDTO pagination)
        {
            var request = new ListaAutorizacaoRecRequest()
            {
                CpfCnpjUsuarioPagador = data.CpfCnpjUsuarioPagador,
                SituacaoRecorrencia = data.SituacaoRecorrencia,
                NomeUsuarioRecebedor = data.NomeUsuarioRecebedor,
                AgenciaUsuarioPagador = data.AgenciaUsuarioPagador,
                ContaUsuarioPagador = data.ContaUsuarioPagador,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };
            try
            {
                var response = await Mediator.Send(request);
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

        [HttpGet("{idAutorizacao}/recorrencia/{idRecorrencia}")]
        [SwaggerOperation(Summary = "Retorna todas as informações de uma solicitação")]
        [SwaggerResponse(200, Type = typeof(TypedApiMetaDataNonPaginatedResponse<AutorizacaoRecorrencia>))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        [RequiredHeaders]
        public async Task<ActionResult> GetDetalheAutorizacao(string idAutorizacao, string idRecorrencia)
        {
            var request = new DetalhesAutorizacaoRecRequest()
            {
                IdAutorizacao = idAutorizacao,
                IdRecorrencia = idRecorrencia
            };
            try
            {
                var response = await Mediator.Send(request);
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
    }
}
