using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pay.Recorrencia.Gestao.Api.Controllers;
using Pay.Recorrencia.Gestao.Application.Commands.SolicitacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.Test
{
    public class SolicitacaoRecorrenciaControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SolicitacaoAutorizacaoRecorrenciaController _controller;

        public SolicitacaoRecorrenciaControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new SolicitacaoAutorizacaoRecorrenciaController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CriarSolicitacaoRecorrenciaAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new IncluirSolicitacaoRecorrenciaCommand
            {
                IdSolicRecorrencia = "123",
                IdRecorrencia = "456",
                TipoRecorrencia = "RCUR",
                TipoFrequencia = "MIAN",
                DataInicialRecorrencia = DateTime.Now,
                SituacaoSolicRecorrencia = "PNDG",
                NomeUsuarioRecebedor = "Test User",
                CpfCnpjUsuarioRecebedor = "12345678901",
                ParticipanteDoUsuarioRecebedor = "Test",
                CpfCnpjUsuarioPagador = "98765432100",
                ContaUsuarioPagador = "12345",
                NumeroContrato = "789",
                DataHoraCriacaoRecorr = DateTime.Now,
                DataHoraCriacaoSolicRecorr = DateTime.Now,
                DataHoraExpiracaoSolicRecorr = DateTime.Now.AddDays(1),
                DataUltimaAtualizacao = DateTime.Now
            };

            var response = new MensagemPadraoResponse(200, "200", string.Empty);

            _mediatorMock.Setup(m => m.Send(request, default)).ReturnsAsync(response);

            // Act
            var result = await _controller.CriarSolicitacaoRecorrenciaAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task CriarSolicitacaoRecorrenciaAsync_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("IdSolicRecorrencia", "Required");

            var request = new IncluirSolicitacaoRecorrenciaCommand();

            // Act
            var result = await _controller.CriarSolicitacaoRecorrenciaAsync(request);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            var response = Assert.IsType<MensagemPadraoResponse>(badRequestResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
            Assert.Equal("Erro de validação nos dados fornecidos.", response.Error.Message);
        }

        [Fact]
        public async Task CriarSolicitacaoRecorrenciaAsync_InternalServerError_Returns500()
        {
            // Arrange
            var request = new IncluirSolicitacaoRecorrenciaCommand
            {
                IdSolicRecorrencia = "123",
                IdRecorrencia = "456",
                TipoRecorrencia = "RCUR",
                TipoFrequencia = "MIAN",
                DataInicialRecorrencia = DateTime.Now,
                SituacaoSolicRecorrencia = "PNDG",
                NomeUsuarioRecebedor = "Test User",
                CpfCnpjUsuarioRecebedor = "12345678901",
                ParticipanteDoUsuarioRecebedor = "Test",
                CpfCnpjUsuarioPagador = "98765432100",
                ContaUsuarioPagador = "12345",
                NumeroContrato = "789",
                DataHoraCriacaoRecorr = DateTime.Now,
                DataHoraCriacaoSolicRecorr = DateTime.Now,
                DataHoraExpiracaoSolicRecorr = DateTime.Now.AddDays(1),
                DataUltimaAtualizacao = DateTime.Now
            };

            _mediatorMock.Setup(m => m.Send(request, default)).ThrowsAsync(new Exception("Erro interno no servidor."));

            // Act
            var result = await _controller.CriarSolicitacaoRecorrenciaAsync(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var response = Assert.IsType<MensagemPadraoResponse>(objectResult.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
            Assert.Equal("Erro interno no servidor.", response.Error.Message);
        }

        [Fact]
        public async Task AtualizarSolicitacaoRecorrenciaAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var idSolicRecorrencia = "123";
            var request = new AtualizarSolicitacaoRecorrenciaCommand
            {
                IdSolicRecorrencia = idSolicRecorrencia,
                IdAutorizacao = "456",
                SituacaoSolicRecorrencia = "PNDG",
                DataUltimaAtualizacao = DateTime.Now
            };

            var response = new MensagemPadraoResponse(StatusCodes.Status200OK, StatusCodes.Status200OK.ToString(), string.Empty);

            _mediatorMock.Setup(m => m.Send(request, default)).ReturnsAsync(response);

            // Act
            var result = await _controller.AtualizarSolicitacaoRecorrenciaAsync(idSolicRecorrencia, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task AtualizarSolicitacaoRecorrenciaAsync_NotFound_ReturnsNotFound()
        {
            // Arrange
            var idSolicRecorrencia = "123";
            var request = new AtualizarSolicitacaoRecorrenciaCommand
            {
                IdSolicRecorrencia = idSolicRecorrencia,
                IdAutorizacao = "456",
                SituacaoSolicRecorrencia = "PNDG",
                DataUltimaAtualizacao = DateTime.Now
            };

            var response = new MensagemPadraoResponse(StatusCodes.Status404NotFound, "ERRO-PIXAUTO-005", "Solicitação não encontrada.");

            _mediatorMock.Setup(m => m.Send(request, default)).ReturnsAsync(response);

            // Act
            var result = await _controller.AtualizarSolicitacaoRecorrenciaAsync(idSolicRecorrencia, request);

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("ERRO-PIXAUTO-005", response.Error.Code);
            Assert.Equal("Solicitação não encontrada.", response.Error.Message);
        }

        [Fact]
        public async Task AtualizarSolicitacaoRecorrenciaAsync_InternalServerError_Returns500()
        {
            // Arrange
            var idSolicRecorrencia = "123";
            var request = new AtualizarSolicitacaoRecorrenciaCommand
            {
                IdSolicRecorrencia = idSolicRecorrencia,
                IdAutorizacao = "456",
                SituacaoSolicRecorrencia = "PNDG",
                DataUltimaAtualizacao = DateTime.Now
            };

            _mediatorMock.Setup(m => m.Send(request, default)).ThrowsAsync(new Exception("Erro interno no servidor."));

            // Act
            var result = await _controller.AtualizarSolicitacaoRecorrenciaAsync(idSolicRecorrencia, request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var response = Assert.IsType<MensagemPadraoResponse>(objectResult.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
            Assert.Equal("Erro interno no servidor.", response.Error.Message);
        }
    }
}
