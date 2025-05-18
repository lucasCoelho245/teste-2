using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pay.Recorrencia.Gestao.Api.Controllers;
using Pay.Recorrencia.Gestao.Application.Services;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Xunit;

namespace Pay.Recorrencia.Gestao.Test;

public class JornadaControllerTests
{
    private readonly Mock<IJornadaService> _serviceMock;
    private readonly JornadaController _controller;

    public JornadaControllerTests()
    {
        _serviceMock = new Mock<IJornadaService>();
        _controller = new JornadaController(_serviceMock.Object);
    }

    [Fact]
    public async Task Get_AllJornadas_ReturnsOkWithList()
    {
        var lista = new List<JornadaDto>
        {
            new() { TpJornada="J1", IdRecorrencia="R1" },
            new() { TpJornada="J2", IdRecorrencia="R2" }
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(lista);

        var result = await _controller.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.Equal(lista, ok.Value);
    }

    [Fact]
    public async Task GetByRecorrencia_MissingParams_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("tpJornada", "required");
        _controller.ModelState.AddModelError("idRecorrencia", "required");

        var result = await _controller.GetByRecorrencia(null, null);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, bad.StatusCode);
    }

    [Fact]
    public async Task GetByRecorrencia_ValidParams_ReturnsOkWithDto()
    {
        var dto = new JornadaDto
        {
            TpJornada = "J1",
            IdRecorrencia = "R1",
            IdE2E = "E1",
            IdConciliacaoRecebedor = "C1",
            SituacaoJornada = "Ativa",
            DtAgendamento = DateTime.Parse("2025-05-13T10:00:00"),
            VlAgendamento = 100.5m,
            DtPagamento = DateTime.Parse("2025-05-14T10:00:00"),
            DataHoraCriacao = DateTime.Parse("2025-05-05T10:00:00"),
            DataUltimaAtualizacao = DateTime.Parse("2025-05-15T10:00:00")
        };
        _serviceMock
            .Setup(s => s.GetByJornadaERecorrenciaAsync("J1", "R1"))
            .ReturnsAsync(dto);

        var result = await _controller.GetByRecorrencia("J1", "R1");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, ok.Value);
    }

    [Fact]
    public async Task GetByAgendamento_MissingParams_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("tpJornada", "required");
        _controller.ModelState.AddModelError("idE2E", "required");

        var result = await _controller.GetByAgendamento(null, null);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, bad.StatusCode);
    }

    [Fact]
    public async Task GetByAgendamento_ValidParams_ReturnsOkWithDto()
    {
        var dto = new JornadaDto { TpJornada="AGND", IdE2E="E1" };
        _serviceMock
            .Setup(s => s.GetByJornadaE2EAsync("AGND", "E1"))
            .ReturnsAsync(dto);

        var result = await _controller.GetByAgendamento("AGND", "E1");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, ok.Value);
    }

    [Fact]
    public async Task GetByFiltros_AllOptional_ReturnsOkWithList()
    {
        var lista = new List<JornadaDto>
        {
            new() { TpJornada="X", IdRecorrencia="Y" }
        };
        _serviceMock
            .Setup(s => s.GetByAnyFilterAsync(null, null, null, null))
            .ReturnsAsync(lista);

        var result = await _controller.GetByFiltros(null, null, null, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lista, ok.Value);
    }
}