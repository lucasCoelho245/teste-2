using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pay.Recorrencia.Gestao.Api.Controllers;
using Pay.Recorrencia.Gestao.Application.Commands.ConsultaDetalheDadosCobranca;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Test
{
    public class ConsultaCobrancaControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsultaCobrancaController _cobrancaController;

        public ConsultaCobrancaControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _cobrancaController = new ConsultaCobrancaController(_mediatorMock.Object);
        }

        [Fact]
        public async Task ConsultaDetalheDadosCobranca_ValidRequest_ReturnsOk()
        {
            var command = new ConsultaDetalheDadosCobrancaCommand
            {
                AgenciaUsuarioPagador = "0001",
                IdTipoContaPagador = "CACC",
                ContaUsuarioPagador = "893245",
                IdRecorrencia = "20001223",
                IdOperacao = "234"
            };

            var expectedResponse = new DetalheDadosCobranca
            {
                IdOperacao = "12345",
                IdRecorrencia = "67890",
                VlOperacao = 150.75m,
                DtPagto = DateTime.Parse("2025-05-21T00:00:00"),
                NomeUsuarioRecebedor = "João Silva",
                CpfCnpjUsuarioRecebedor = "12345678901",
                ParticipanteDoUsuarioRecebedor = "Banco XYZ",
                CpfCnpjUsuarioPagador = "98765432100",
                NumeroContrato = "CONTRATO789",
                DescObjetoContrato = "Serviço de assinatura mensal"
            };

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(expectedResponse);

            var response = await _cobrancaController.ConsultaDetalheDadosCobranca(command);
            var okResult = Assert.IsType<OkObjectResult>(response);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task ConsultaDetalheDadosCobranca_InvalidRequest_ReturnsBadRequest()
        {
            var command = new ConsultaDetalheDadosCobrancaCommand
            {
                AgenciaUsuarioPagador = "0001",
                IdTipoContaPagador = "CACC",
                ContaUsuarioPagador = "893245",
                IdRecorrencia = "20001223",
                IdOperacao = "234"
            };

            var expectedResponse = new MensagemPadraoResponse(StatusCodes.Status400BadRequest, "ERRO-PIXAUTO-017", "Campos obrigatorios preenchidos de forma incorreta ou vazios");

            _mediatorMock.Setup(m => m.Send(command, default)).ThrowsAsync(new ArgumentException("ERRO-PIXAUTO-017"));

            var response = await _cobrancaController.ConsultaDetalheDadosCobranca(command);
            var badRequest = Assert.IsType<BadRequestObjectResult>(response);
            var responseValue = Assert.IsType<MensagemPadraoResponse>(badRequest.Value);

            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal(expectedResponse.Error.Code, responseValue.Error.Code);
            Assert.Equal(expectedResponse.Error.Message, responseValue.Error.Message);
        }

        [Fact]
        public async Task ConsultaDetalheDadosCobranca_InvalidRequest_ReturnsInternalServerError()
        {
            var command = new ConsultaDetalheDadosCobrancaCommand
            {
                AgenciaUsuarioPagador = "0001",
                IdTipoContaPagador = "CACC",
                ContaUsuarioPagador = "893245",
                IdRecorrencia = "20001223",
                IdOperacao = "234"
            };

            var expectedResponse = new MensagemPadraoResponse(StatusCodes.Status500InternalServerError, "", "Erro interno do servidor");

            _mediatorMock.Setup(m => m.Send(command, default)).ThrowsAsync(new Exception());

            var response = await _cobrancaController.ConsultaDetalheDadosCobranca(command);
            var error = Assert.IsType<ObjectResult>(response);
            var responseValue = Assert.IsType<MensagemPadraoResponse>(error.Value);

            Assert.Equal(500, error.StatusCode);
            Assert.Equal(expectedResponse.Error.Code, responseValue.Error.Code);
            Assert.Equal(expectedResponse.Error.Message, responseValue.Error.Message);
        }
    }
}
