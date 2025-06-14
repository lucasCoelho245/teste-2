﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Commands.ControleJornada;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Detalhes;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista;
using Pay.Recorrencia.Gestao.Application.Query.SolicAutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Application.Services;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/pix-automatico/jornadas")]
public class JornadaController : ControllerBase
{
    private IMediator _mediator { get; }
    public JornadaController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Retorna jornada")]
    [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<JornadaList>))]
    [SwaggerResponse(404, Type = typeof(ErrorResponse))]
    [Produces("application/json")]
    public async Task<IActionResult> GetAllAsync([FromQuery] ListaControleJornadaRequest request)
    {
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

    [HttpGet("autorizacao")]
    [SwaggerOperation(Summary = "Retorna jornada")]
    [SwaggerResponse(200, Type = typeof(ApiMetaDataNonPaginatedResponse<Jornada>))]
    [SwaggerResponse(404, Type = typeof(ErrorResponse))]
    [Produces("application/json")]
    public async Task<IActionResult> GetByRecorrenciaAsync([FromQuery] DetalhesControleJornadaAutorizacaoResquest request)
    {
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

    [HttpGet("agendamento")]
    [SwaggerOperation(Summary = "Retorna jornada")]
    [SwaggerResponse(200, Type = typeof(ApiMetaDataNonPaginatedResponse<Jornada>))]
    [SwaggerResponse(404, Type = typeof(ErrorResponse))]
    [Produces("application/json")]
    public async Task<IActionResult> GetByAnyFilterAsync([FromQuery] DetalhesControleJornadaAgendamentoResquest request)
    {

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

    [HttpGet("autorizacao-agendamento")]
    [SwaggerOperation(Summary = "Retorna jornada")]
    [SwaggerResponse(200, Type = typeof(TypedApiMetaDataPaginatedResponse<Jornada>))]
    [SwaggerResponse(404, Type = typeof(ErrorResponse))]
    [Produces("application/json")]
    public async Task<IActionResult> GetByAnyFilterAsync([FromQuery] ListaControleJornadaAgendamentoAutorizacaoRequest request)
    {
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

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] AtualizarControleJornadaCommand command)
    {
        MensagemPadraoResponse response;
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new MensagemPadraoResponse(StatusCodes.Status400BadRequest, string.Empty, "Erro de validação nos dados fornecidos."));

            response = await _mediator.Send(command);

            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new MensagemPadraoResponse(StatusCodes.Status500InternalServerError, string.Empty, ex.Message.ToString()));
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] IncluirControleJornadaCommand command)
    {
        MensagemPadraoResponse response;
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new MensagemPadraoResponse(StatusCodes.Status400BadRequest, string.Empty, "Erro de validação nos dados fornecidos."));

            response = await _mediator.Send(command);

            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new MensagemPadraoResponse(StatusCodes.Status500InternalServerError, string.Empty, ex.Message.ToString()));
            throw;
        }
    }
}