using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pay.Recorrencia.Gestao.Api.Controllers;
using Pay.Recorrencia.Gestao.Application.Commands.TemplateMensagem;
using Pay.Recorrencia.Gestao.Application.Response;

namespace Pay.Recorrencia.Gestao.UnitTest
{
    public class TemplateMensagemTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TemplateMensagemController _controller;

        public TemplateMensagemTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TemplateMensagemController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetTemplateMensagem_InvalidRequest_ReturnsNotFound_WhenNoDetailsAreFound()
        {
            var command = new ConsultaTemplateMensagemCommand
            {
                IdMensagem = null
            };

            var expectedResponse = new MensagemPadraoResponse(StatusCodes.Status404NotFound, "ERRO-PIXAUTO-024", "Registro de Mensagem não encontrado");

            _mediatorMock.Setup(m => m.Send(command, default)).ThrowsAsync(new ArgumentException("ERRO-PIXAUTO-024"));

            var response = await _controller.GetTemplateMensagem(command.IdMensagem);
            var notFound = Assert.IsType<NotFoundObjectResult>(response);
            var responseValue = Assert.IsType<MensagemPadraoResponse>(notFound.Value);

            Assert.Equal(404, notFound.StatusCode);
            Assert.Equal(expectedResponse.Error.Code, responseValue.Error.Code);
            Assert.Equal(expectedResponse.Error.Message, responseValue.Error.Message);
        }

        [Fact]
        public async Task InsertTemplateAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new IncluirTemplateMensagemCommand
            {
                IdMensagem = "MSG-PIXAUTO-21",
                TxTemplate = "A autorização do Pix Automático está ok."
            };

            var response = new MensagemPadraoResponse(200, "200", string.Empty);

            _mediatorMock.Setup(m => m.Send(request, default)).ReturnsAsync(response);

            // Act
            var result = await _controller.InsertTemplateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task InsertTemplateAsync_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("IdMensagem", "Required");

            var request = new IncluirTemplateMensagemCommand();

            // Act
            var result = await _controller.InsertTemplateAsync(request);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            var response = Assert.IsType<MensagemPadraoResponse>(badRequestResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
            Assert.Equal("Campos não preenchidos corretamente.", response.Error.Message);
        }

        [Fact]
        public async Task InsertTemplateAsync_InternalServerError_Returns500()
        {
            // Arrange
            var request = new IncluirTemplateMensagemCommand
            {
                IdMensagem = "MSG-PIXAUTO-21",
                TxTemplate = "A autorização do Pix Automático está ok."
            };

            _mediatorMock.Setup(m => m.Send(request, default)).ThrowsAsync(new Exception("Erro interno no servidor."));

            // Act
            var result = await _controller.InsertTemplateAsync(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var response = Assert.IsType<MensagemPadraoResponse>(objectResult.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
            Assert.Equal("Erro interno no servidor.", response.Error.Message);
        }
    }
}
