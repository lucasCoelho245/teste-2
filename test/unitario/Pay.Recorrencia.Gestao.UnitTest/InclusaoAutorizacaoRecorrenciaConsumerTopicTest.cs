using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pay.Recorrencia.Gestao.Application.Commands.IncluirAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Consumer.Models;
using Pay.Recorrencia.Gestao.Consumer.Worker.Consumer.InclusaoAutorizacaoRecorrencia;
using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Services;
using System.Text.Json;

namespace Pay.Recorrencia.Gestao.Test;

public class ConsumerInclusaoAutorizacaoRecorrenciaTopicValidationTest
{
    [Fact]
    public void Consume_ShouldConvertJsonSuccessfully()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ConsumerInclusaoAutorizacaoRecorrenciaTopic>>();
        var kafkaProducerMock = new Mock<IKafkaProducerService>();
        var mediatorMock = new Mock<IMediator>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(x => x.GetService(typeof(IMediator)))
            .Returns(mediatorMock.Object);

        var scopeMock = new Mock<IServiceScope>();
        scopeMock.Setup(x => x.ServiceProvider)
            .Returns(serviceProviderMock.Object);

        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        scopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(scopeMock.Object);

        var kafkaSettingsMock = new Mock<IOptions<InputParametersKafkaConsumer>>();
        kafkaSettingsMock.Setup(x => x.Value).Returns(new InputParametersKafkaConsumer
        {
            Consumer = new InputParameterskafkaConsumer
            {
                KafkaConsumerMappings = new List<KafkaConsumerMapping>
                {
                    new KafkaConsumerMapping { Topic = "teste.v1" }
                }
            }
        });

        var consumer = new ConsumerInclusaoAutorizacaoRecorrenciaTopic(
            loggerMock.Object,
            kafkaProducerMock.Object,
            kafkaSettingsMock.Object,
            scopeFactoryMock.Object);

        var validJson = """
            {
              "idRecorrencia": "string",
              "tipoRecorrencia": "RCUR",
              "tipoFrequencia": "MIAN",
              "dataInicialRecorrencia": "2025-05-01",
              "dataFinalRecorrencia": "2025-12-31",
              "codigoMoedaSolicRecorr": "BRL",
              "valorFixoSolicRecorrencia": 150.00,
              "indicadorValorMin": true,
              "valorMinRecebedorSolicRecorr": 100.00,
              "nomeUsuarioRecebedor": "João da Silva",
              "cpfCnpjUsuarioRecebedor": "12345678900",
              "participanteDoUsuarioRecebedor": "341",
              "cpfCnpjUsuarioPagador": "98765432100",
              "contaUsuarioPagador": 123456,
              "agenciaUsuarioPagador": 7897,
              "participanteDoUsuarioPagador": "001",
              "nomeDevedor": "Empresa XPTO Ltda",
              "cpfCnpjDevedor": "11222333000181",
              "numeroContrato": "CONT-2025-001",
              "descObjetoContrato": "Assinatura mensal de software",
              "tpRetentativa": "PERMITE_3R_7D",
              "dataHoraCriacaoRecorr": "2025-05-01T10:00:00Z",
              "dataUltimaAtualizacao": "2025-05-10T12:30:00Z"
            }
        """;

        // Act
        var result = consumer.ConvertJsonToSolicitacaoAutorizacaoRecorrencia(validJson);

        // Assert
        Assert.NotNull(result); // Passa se o retorno for um objeto (indicando sucesso)
    }

    [Fact]
    public void Consume_ShouldValidateJsonSuccessfully()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ConsumerInclusaoAutorizacaoRecorrenciaTopic>>();
        var kafkaProducerMock = new Mock<IKafkaProducerService>();
        var mediatorMock = new Mock<IMediator>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(x => x.GetService(typeof(IMediator)))
            .Returns(mediatorMock.Object);

        var scopeMock = new Mock<IServiceScope>();
        scopeMock.Setup(x => x.ServiceProvider)
            .Returns(serviceProviderMock.Object);

        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        scopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(scopeMock.Object);

        var kafkaSettingsMock = new Mock<IOptions<InputParametersKafkaConsumer>>();
        kafkaSettingsMock.Setup(x => x.Value).Returns(new InputParametersKafkaConsumer
        {
            Consumer = new InputParameterskafkaConsumer
            {
                KafkaConsumerMappings = new List<KafkaConsumerMapping>
                {
                    new KafkaConsumerMapping { Topic = "teste.v1" }
                }
            }
        });

        var consumer = new ConsumerInclusaoAutorizacaoRecorrenciaTopic(
            loggerMock.Object,
            kafkaProducerMock.Object,
            kafkaSettingsMock.Object,
            scopeFactoryMock.Object);

        SolicitacaoAutorizacaoRecorrencia objeto = new SolicitacaoAutorizacaoRecorrencia
        {
            IdRecorrencia = "string",
            TipoRecorrencia = "RCUR",
            TipoFrequencia = "MIAN",
            DataInicialRecorrencia = new DateTime(2025, 5, 1),
            DataFinalRecorrencia = new DateTime(2025, 12, 31),
            CodigoMoedaSolicRecorr = "BRL",
            ValorFixoSolicRecorrencia = 150.00m,
            IndicadorValorMin = true,
            ValorMinRecebedorSolicRecorr = 100.00m,
            NomeUsuarioRecebedor = "João da Silva",
            CpfCnpjUsuarioRecebedor = "12345678900",
            ParticipanteDoUsuarioRecebedor = "341",
            CpfCnpjUsuarioPagador = "98765432100",
            ContaUsuarioPagador = 123456,
            AgenciaUsuarioPagador = 7897,
            ParticipanteDoUsuarioPagador = "001",
            NomeDevedor = "Empresa XPTO Ltda",
            CpfCnpjDevedor = "11222333000181",
            NumeroContrato = "CONT-2025-001",
            DescObjetoContrato = "Assinatura mensal de software",
            TpRetentativa = "PERMITE_3R_7D",
            DataHoraCriacaoRecorr = DateTime.Parse("2025-05-01T10:00:00Z"),
            DataUltimaAtualizacao = DateTime.Parse("2025-05-10T12:30:00Z")
        };


        // Act
        var result = consumer.ValidateObjectDadosSolicitacao(objeto);

        // Assert
        Assert.True(result); // Passa se o retorno for um True (indicando sucesso)
    }

    [Fact]
    public async Task Consume_ShouldSaveSuccessfully()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ConsumerInclusaoAutorizacaoRecorrenciaTopic>>();
        var kafkaProducerMock = new Mock<IKafkaProducerService>();
        var mediatorMock = new Mock<IMediator>();

        mediatorMock
            .Setup(x => x.Send<Unit>((IRequest<Unit>)It.IsAny<InserirAutorizacaoRecorrenciaCommand>(), default))
            .ReturnsAsync(Unit.Value);


        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IMediator)))
            .Returns(mediatorMock.Object);

        var scopeMock = new Mock<IServiceScope>();
        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);

        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);

        var kafkaSettingsMock = new Mock<IOptions<InputParametersKafkaConsumer>>();
        kafkaSettingsMock.Setup(x => x.Value).Returns(new InputParametersKafkaConsumer
        {
            Consumer = new InputParameterskafkaConsumer
            {
                KafkaConsumerMappings = new List<KafkaConsumerMapping>
                {
                    new KafkaConsumerMapping { Topic = "teste.v1" }
                }
            }
        });

        var consumer = new ConsumerInclusaoAutorizacaoRecorrenciaTopic(
            loggerMock.Object,
            kafkaProducerMock.Object,
            kafkaSettingsMock.Object,
            scopeFactoryMock.Object);

        var objeto = new SolicitacaoAutorizacaoRecorrencia
        {
            IdRecorrencia = "string",
            TipoRecorrencia = "RCUR",
            TipoFrequencia = "MIAN",
            DataInicialRecorrencia = new DateTime(2025, 5, 1),
            DataFinalRecorrencia = new DateTime(2025, 12, 31),
            CodigoMoedaSolicRecorr = "BRL",
            ValorFixoSolicRecorrencia = 150.00m,
            IndicadorValorMin = true,
            ValorMinRecebedorSolicRecorr = 100.00m,
            NomeUsuarioRecebedor = "João da Silva",
            CpfCnpjUsuarioRecebedor = "12345678900",
            ParticipanteDoUsuarioRecebedor = "341",
            CpfCnpjUsuarioPagador = "98765432100",
            ContaUsuarioPagador = 123456,
            AgenciaUsuarioPagador = 7897,
            ParticipanteDoUsuarioPagador = "001",
            NomeDevedor = "Empresa XPTO Ltda",
            CpfCnpjDevedor = "11222333000181",
            NumeroContrato = "CONT-2025-001",
            DescObjetoContrato = "Assinatura mensal de software",
            TpRetentativa = "PERMITE_3R_7D",
            DataHoraCriacaoRecorr = DateTime.Parse("2025-05-01T10:00:00Z"),
            DataUltimaAtualizacao = DateTime.Parse("2025-05-10T12:30:00Z")
        };

        // Act
        await consumer.ArmazenaDadosDaSolicitacao(objeto, "AR2025052000000000003");

        //// Assert
        //mediatorMock.Verify(x =>
        //    x.Send<AutorizacaoRecorrencia>(
        //        It.Is<InserirAutorizacaoRecorrenciaCommand>(cmd =>
        //            cmd.IdAutorizacao.ToString() == "AR2025052000000000003"
        //        ),
        //        It.IsAny<CancellationToken>()),
        //    Times.Once
        //);

    }


    [Fact]
    public async Task Consume_ShouldSendToProducerSuccessfullyAsync()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ConsumerInclusaoAutorizacaoRecorrenciaTopic>>();
        var kafkaProducerMock = new Mock<IKafkaProducerService>();
        var mediatorMock = new Mock<IMediator>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(x => x.GetService(typeof(IMediator)))
            .Returns(mediatorMock.Object);

        var scopeMock = new Mock<IServiceScope>();
        scopeMock.Setup(x => x.ServiceProvider)
            .Returns(serviceProviderMock.Object);

        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        scopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(scopeMock.Object);

        var kafkaSettingsMock = new Mock<IOptions<InputParametersKafkaConsumer>>();
        kafkaSettingsMock.Setup(x => x.Value).Returns(new InputParametersKafkaConsumer
        {
            Consumer = new InputParameterskafkaConsumer
            {
                KafkaConsumerMappings = new List<KafkaConsumerMapping>
                {
                    new KafkaConsumerMapping { Topic = "teste.v1" }
                }
            }
        });


        var consumer = new ConsumerInclusaoAutorizacaoRecorrenciaTopic(
            loggerMock.Object,
            kafkaProducerMock.Object,
            kafkaSettingsMock.Object,
            scopeFactoryMock.Object);

        SolicitacaoAutorizacaoRecorrencia objeto = new SolicitacaoAutorizacaoRecorrencia
        {
            IdRecorrencia = "string",
            TipoRecorrencia = "RCUR",
            TipoFrequencia = "MIAN",
            DataInicialRecorrencia = new DateTime(2025, 5, 1),
            DataFinalRecorrencia = new DateTime(2025, 12, 31),
            CodigoMoedaSolicRecorr = "BRL",
            ValorFixoSolicRecorrencia = 150.00m,
            IndicadorValorMin = true,
            ValorMinRecebedorSolicRecorr = 100.00m,
            NomeUsuarioRecebedor = "João da Silva",
            CpfCnpjUsuarioRecebedor = "12345678900",
            ParticipanteDoUsuarioRecebedor = "341",
            CpfCnpjUsuarioPagador = "98765432100",
            ContaUsuarioPagador = 123456,
            AgenciaUsuarioPagador = 7897,
            ParticipanteDoUsuarioPagador = "001",
            NomeDevedor = "Empresa XPTO Ltda",
            CpfCnpjDevedor = "11222333000181",
            NumeroContrato = "CONT-2025-001",
            DescObjetoContrato = "Assinatura mensal de software",
            TpRetentativa = "PERMITE_3R_7D",
            DataHoraCriacaoRecorr = DateTime.Parse("2025-05-01T10:00:00Z"),
            DataUltimaAtualizacao = DateTime.Parse("2025-05-10T12:30:00Z")
        };

        // Act
        var responseMessage = new RecorrenciaIncluidaDTO(objeto);

        consumer.RelayEventHubMessage("teste.producer.v1", JsonSerializer.Serialize(responseMessage));
        kafkaProducerMock.Verify(
            x => x.SendMessageWithParameterAsync(
                "teste.producer.v1",
                It.Is<string>(m => m.Contains("Jo\\u00E3o da Silva"))
            ),
            Times.Once
        );
    }
}
