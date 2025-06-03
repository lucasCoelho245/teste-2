using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pay.Recorrencia.Gestao.Api.Controllers;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrenciaCanal;
using Pay.Recorrencia.Gestao.Application.Response;

public class AlterarAutorizacaoRecorrenciaCanalControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AutorizacaoRecorrenciaController _controller;

    public AlterarAutorizacaoRecorrenciaCanalControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AutorizacaoRecorrenciaController(_mediatorMock.Object);
    }

    [Fact]
    public async Task AlterarAutorizacaoCanal_DeveRetornarOk_QuandoSucesso()
    {
        var command = new AlterarAutorizacaoRecorrenciaCanalCommand
        {
            IdAutorizacao = Guid.NewGuid().ToString(),
            IdRecorrencia = Guid.NewGuid().ToString(),
            FlagValorMaximoAutorizado = true,
            ValorMaximoAutorizado = 100.00m,
            FlagPermiteNotificacao = true
        };

        var expectedResponse = new ApiSimpleResponse("OK", "Alterado com sucesso");

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.AlterarAutorizacaoCanal(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, okResult.Value);
    }

    [Fact]
    public async Task AlterarAutorizacaoCanal_DeveRetornarNotFound_QuandoRespostaNula()
    {
        var command = new AlterarAutorizacaoRecorrenciaCanalCommand
        {
            IdAutorizacao = Guid.NewGuid().ToString(),
            IdRecorrencia = Guid.NewGuid().ToString(),
            FlagValorMaximoAutorizado = false,
            ValorMaximoAutorizado = 0m,
            FlagPermiteNotificacao = false
        };


        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApiSimpleResponse)null);

        var result = await _controller.AlterarAutorizacaoCanal(command);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ApiSimpleResponse>(notFoundResult.Value);
        Assert.Equal("NOK", response.CodigoRetorno);
    }

    [Fact]
    public async Task AlterarAutorizacaoCanal_DeveRetornarBadRequest_QuandoExcecaoLancada()
    {
        var command = new AlterarAutorizacaoRecorrenciaCanalCommand
        {
            IdAutorizacao = Guid.NewGuid().ToString(),
            IdRecorrencia = Guid.NewGuid().ToString(),
            FlagValorMaximoAutorizado = true,
            ValorMaximoAutorizado = 200m,
            FlagPermiteNotificacao = true
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new System.Exception("Erro inesperado"));

        var result = await _controller.AlterarAutorizacaoCanal(command);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ApiSimpleResponse>(badRequestResult.Value);
        Assert.Equal("NOK", response.CodigoRetorno);
        Assert.Equal("Erro inesperado", response.MensagemRetorno);
    }
}