using MediatR;
using Moq;
using Pay.Recorrencia.Gestao.Api.Controllers;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Application.Commands.AlterarAutorizacaoRecorrenciaCanal;
using Pay.Recorrencia.Gestao.Application.Query.AutorizacaoRec.Detalhes;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Domain.Enums;

namespace Pay.Recorrencia.Gestao.UnitTest.AutorizacaoRecorrencia
{
    public class AlterarAutorizacaoRecorrenciaCanalHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SolicitacaoAutorizacaoRecorrenciaController _controller;

        public AlterarAutorizacaoRecorrenciaCanalHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new SolicitacaoAutorizacaoRecorrenciaController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoAutorizacaoNaoForEncontrada()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<DetalhesAutorizacaoRecRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DetalhesAutorizacaoRecResponse?)null);

            var handler = new AlterarAutorizacaoRecorrenciaCanalHandler(mediatorMock.Object);
            var command = new AlterarAutorizacaoRecorrenciaCanalCommand
            {
                IdAutorizacao = Guid.NewGuid().ToString(),
                IdRecorrencia = Guid.NewGuid().ToString()
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal("NOK", result.CodigoRetorno);
            Assert.Equal("ERRO-PIXAUTO-011: Autorização de recorrência não encontrada.", result.MensagemRetorno);
        }

        [Fact]
        public async Task Handle_DeveAlterarAutorizacaoComSucesso_QuandoDadosValidosForemRecebidos()
        {
            var mediatorMock = new Mock<IMediator>();

            var detalhes = new Pay.Recorrencia.Gestao.Domain.Entities.AutorizacaoRecorrencia
            {
                IdAutorizacao = Guid.NewGuid().ToString(),
                ValorMaximoAutorizado = 100,
                FlagValorMaximoAutorizado = true,
                FlagPermiteNotificacao = true,
                MotivoRejeicaoRecorrencia = null,
                CodigoSituacaoCancelamentoRecorrencia = null,
                SituacaoRecorrencia = SituacaoRecorrencia.CFDB.ToString(),
                TipoSituacaoRecorrencia = TipoSituacaoRecorrencia.AUT1.ToString(),
                DataHoraCriacaoRecorr = DateTime.UtcNow
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<DetalhesAutorizacaoRecRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DetalhesAutorizacaoRecResponse { Data = detalhes });

            var mensagemResponse = new MensagemPadraoResponse(200, "OK", "Autorização atualizada com sucesso");
            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAutorizacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mensagemResponse); // <-- CORRIGIDO

            var handler = new AlterarAutorizacaoRecorrenciaCanalHandler(mediatorMock.Object);
            var command = new AlterarAutorizacaoRecorrenciaCanalCommand
            {
                IdAutorizacao = detalhes.IdAutorizacao,
                IdRecorrencia = Guid.NewGuid().ToString()
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal("OK", result.CodigoRetorno);
            Assert.Equal("Autorização atualizada com sucesso", result.MensagemRetorno);
        }

    }
}