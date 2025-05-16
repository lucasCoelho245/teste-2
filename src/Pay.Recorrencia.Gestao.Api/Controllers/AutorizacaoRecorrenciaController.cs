using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost()]
        [SwaggerOperation(Summary = "Insere a Autorização Recorrência", Description = "Insere uma nova Autorização Recorrência.")]
        [SwaggerResponse(200, Type = typeof(AutorizacaoRecorrencia))]
        [SwaggerResponse(400)]
        public async Task<ActionResult> InsereDadosAutorizacaoRecorrencia(InserirAutorizacaoRecorrenciaCommand command)
        {
            try
            {
                if (ValidaInformacoesRecebidas(command))
                {
                    var response = await Mediator.Send(command);
                    //Logger.Information("Response: {@Response}", response);
                    if (response is null)
                    {
                        //Logger.Warning("Autorização Recorrência não gravada.");
                        return BadRequest();
                    }

                    //Logger.Information("Autorização Recorrência gravada com sucesso. {@Response}", response);
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Campos não preenchidos corretamente.");
                }
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



        private bool ValidaInformacoesRecebidas(InserirAutorizacaoRecorrenciaCommand autorizacaoRecorrenciaCommand)
        {
            if (autorizacaoRecorrenciaCommand.IdAutorizacao.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.IdRecorrencia.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.SituacaoRecorrencia.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.TipoRecorrencia.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.TipoFrequencia.ToString() == string.Empty)
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

            if (autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.DataHoraCriacaoRecorr.ToString() == string.Empty)
                return false;

            if (autorizacaoRecorrenciaCommand.FlagPermiteNotificacao.ToString() == "")
                return false;

            if (autorizacaoRecorrenciaCommand.FlagValorMaximoAutorizado.ToString() == "")
                return false;

            if (!autorizacaoRecorrenciaCommand.TipoRecorrencia.ToString().Equals("RCUR"))
                return false;

            if (autorizacaoRecorrenciaCommand.TipoFrequencia.ToString().Equals("MIAN")
                && autorizacaoRecorrenciaCommand.TipoFrequencia.ToString().Equals("MNTH")
                && autorizacaoRecorrenciaCommand.TipoFrequencia.ToString().Equals("QURT")
                && autorizacaoRecorrenciaCommand.TipoFrequencia.ToString().Equals("WEEK")
                && autorizacaoRecorrenciaCommand.TipoFrequencia.ToString().Equals("YEAR"))
            {
                return false;
            }

            if (!autorizacaoRecorrenciaCommand.CodigoMoedaAutorizacaoRecorrencia.ToString().Equals("BRL"))
                return false;


            if (autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia.Equals("CRTN")
                && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia.Equals("AUT1")
                && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia.Equals("AUT2")
                && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia.Equals("AUT3")
                && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia.Equals("AUT4")
                && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia.Equals("CFDB")
                && autorizacaoRecorrenciaCommand.TipoSituacaoRecorrencia.Equals("CCLD"))
            {
                return false;
            }
            if (autorizacaoRecorrenciaCommand.TpRetentativa.Equals("NAO_PERMITE") && autorizacaoRecorrenciaCommand.TpRetentativa.Equals("PERMITE_3R_7D"))
                return false;

            return true;
        }
    }
}
