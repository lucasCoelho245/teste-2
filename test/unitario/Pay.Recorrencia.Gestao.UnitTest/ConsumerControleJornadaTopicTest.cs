using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pay.Recorrencia.Gestao.Application.Commands.ControleJornada;
using Pay.Recorrencia.Gestao.Application.Query.ControleJornada.Lista;
using Pay.Recorrencia.Gestao.Application.Response;
using Pay.Recorrencia.Gestao.Consumer.Worker;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.ControleJornada;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Producer.Models;
using System.Text.Json;

namespace Pay.Recorrencia.Gestao.UniTest
{
    public class ConsumerControleJornadaTopicTests
    {
        // Mocks
        private readonly Mock<ILogger<ConsumerControleJornadaTopic>> _loggerMock = new();
        private readonly Mock<IKafkaProducerService> _kafkaProducerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
        private readonly Mock<IServiceScope> _scopeMock = new();
        private readonly Mock<IServiceProvider> _providerMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();

        private readonly ConsumerControleJornadaTopic _consumer;

        public ConsumerControleJornadaTopicTests()
        {
            var options = Options.Create(new InputParametersKafkaProducer
            {
                Producer = new InputParameterskafkaProducer { Topic = "tdc-saida", ApplicationName = "TestApplication" }
            });

            _scopeMock.Setup(s => s.ServiceProvider).Returns(_providerMock.Object);
            _scopeFactoryMock.Setup(f => f.CreateScope()).Returns(_scopeMock.Object);
            _providerMock.Setup(p => p.GetService(typeof(IMediator))).Returns(_mediatorMock.Object);

            _consumer = new ConsumerControleJornadaTopic(
                _loggerMock.Object,
                options,
                _kafkaProducerMock.Object,
                _mapperMock.Object,
                _scopeFactoryMock.Object
            );
        }

        private string GerarMensagemJson(ControleJornadaEntrada entrada)
        {
            return JsonSerializer.Serialize(entrada);
        }

        private ControleJornadaEntrada CriarEntradaValida()
        {
            return new ControleJornadaEntrada
            {
                IdE2E = "AGNT",
                IdRecorrencia = "rec-001",
                TpJornada = "TIPO",
                SituacaoJornada = "PENDENTE",
                DtAgendamento = DateTime.Today,
                DtPagamento = DateTime.Today.AddDays(1),
                DataUltimaAtualizacao = DateTime.Now,
                IdConciliacaoRecebedor = "AGND"
            };
        }

        [Fact(DisplayName = "Cenário 03: Nenhuma jornada encontrada, deve incluir nova")]
        public async Task Quando_NaoExisteJornada_DeveIncluirNova()
        {
            // Arrange
            //var entrada = CriarEntradaValida();
            //var json = GerarMensagemJson(entrada);

            ////_mediatorMock.Setup(m => m.Send(It.IsAny<ListaControleJornadaRequest>(), default))
            ////    .ReturnsAsync(new List<ControleJornadaEntrada>());

            //_mediatorMock.Setup(m => m.Send(It.IsAny<IncluirControleJornadaCommand>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(new MensagemPadraoResponse(200, "0", null ));

            //// Act
            //await _consumer.ConsumeAsync("topic", 0, json, null, "topic-dev", 1);

            //// Assert
            //_mediatorMock.Verify(m => m.Send(It.IsAny<IncluirControleJornadaCommand>(), default), Times.Once);
        }

        [Fact(DisplayName = "Cenário 04: Jornada existente deve ser atualizada")]
        public async Task Quando_JornadaExistente_DeveAtualizar()
        {
            // Arrange
            //var entrada = CriarEntradaValida();
            //entrada.DataUltimaAtualizacao = DateTime.Now.AddMinutes(10);

            //var existente = CriarEntradaValida();
            //existente.DataUltimaAtualizacao = DateTime.Now;

            ////_mediatorMock.Setup(m => m.Send(It.IsAny<ListaControleJornadaRequest>(), default))
            ////    .ReturnsAsync(new List<ControleJornadaEntrada> { existente });

            //_mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarControleJornadaCommand>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(new MensagemPadraoResponse (200, "0", null));

            //var json = GerarMensagemJson(entrada);

            //// Act
            //await _consumer.ConsumeAsync("topic", 0, json, null, "topic-dev", 1);

            //// Assert
            //_mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarControleJornadaCommand>(), default), Times.Once);
        }

        [Fact(DisplayName = "Cenário 05: Mais de uma jornada encontrada, deve publicar erro")]
        public async Task Quando_MaisDeUmaJornada_DevePublicarErro()
        {
            // Arrange
            //var entrada = CriarEntradaValida(); // garante que required estão setados
            //var json = GerarMensagemJson(entrada);

            ////_mediatorMock.Setup(m => m.Send(It.IsAny<ListaControleJornadaRequest>(), default))
            ////    .ReturnsAsync(new List<ControleJornadaEntrada> { entrada, entrada });

            //_mapperMock.Setup(m => m.Map<MensagemControleJornada>(It.IsAny<ControleJornadaEntrada>()))
            //    .Returns((ControleJornadaEntrada entrada) => new MensagemControleJornada
            //    {
            //        // Copiando as propriedades herdadas da entrada
            //        IdFimAFim = entrada.IdFimAFim,
            //        IdRecorrencia = entrada.IdRecorrencia,
            //        TpJornada = entrada.TpJornada,
            //        SituacaoJornada = entrada.SituacaoJornada,
            //        DtAgendamento = entrada.DtAgendamento,
            //        DtPagamento = entrada.DtPagamento,
            //        DataUltimaAtualizacao = entrada.DataUltimaAtualizacao,

            //        // Propriedades específicas de MensagemControleJornada
            //        CodigoDeErro = null,
            //        DtErro = DateTime.UtcNow
            //    });


            //_kafkaProducerMock.Setup(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns(Task.CompletedTask);

            //// Act
            //await _consumer.ConsumeAsync("topic", 0, json, null, "topic-dev", 1);

            //// Assert
            //_kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Cenário Falha: JSON inválido deve ser ignorado")]
        public async Task Quando_JsonInvalido_DeveIgnorar()
        {
            // Arrange
            var json = "{ invalido }";

            // Act
            await _consumer.ConsumeAsync("tdc-entrada", 0, json, null, "topic-dev", 1);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error, // CORRETO: o nível é Error
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v != null &&
                        v.ToString().Contains("Erro: A mensagem JSON não pôde ser convertida para SolicitacaoRecorrencia.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact(DisplayName = "Cenário 04: Dados antigos não devem atualizar")]
        public async Task Quando_DadosAntigos_DeveIgnorarAtualizacao()
        {
            // Arrange
            var entrada = CriarEntradaValida();
            entrada.DataUltimaAtualizacao = DateTime.Now.AddMinutes(-5);

            var existente = CriarEntradaValida();
            existente.DataUltimaAtualizacao = DateTime.Now;

            //_mediatorMock.Setup(m => m.Send(It.IsAny<ListaControleJornadaRequest>(), default))
            //    .ReturnsAsync(new List<ControleJornadaEntrada> { existente });

            var json = GerarMensagemJson(entrada);

            // Act
            await _consumer.ConsumeAsync("topic", 0, json, null, "topic-dev", 1);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarControleJornadaCommand>(), default), Times.Never);
        }

        [Fact(DisplayName = "Cenário Falha: Quando não achar o topic")]
        public async Task Quando_NaoAchar_O_Topic()
        {
            var loggerMock = new Mock<ILogger<Worker>>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var kafkaSettingsMock = new Mock<IOptions<Consumer.Models.InputParametersKafkaConsumer>>();

            // Arrange
            kafkaSettingsMock.Setup(x => x.Value).Returns(new Consumer.Models.InputParametersKafkaConsumer
            {
                Consumer = null
            });


            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            serviceScopeFactoryMock
                .Setup(x => x.CreateScope())
                .Returns(serviceScopeMock.Object);

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactoryMock.Object);

            var worker = new Worker(loggerMock.Object, serviceProviderMock.Object, kafkaSettingsMock.Object);

            // Act
            await worker.StartAsync(CancellationToken.None);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains("Nenhum mapeamento de consumidor configurado.") == true),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }
    }
}
