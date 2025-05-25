using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer;
using Pay.Recorrencia.Gestao.Consumer.Models;
using Pay.Recorrencia.Gestao.Consumer.Worker;

namespace Pay.Recorrencia.Gestao.Test;

public class ConsumerPayPagamentoProcessadoTopicTest
{
    [Fact]
    public async Task ExecuteAsync_ShouldLogWarning_WhenConsumedTopicsIsEmpty()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Worker>>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var kafkaSettingsMock = new Mock<IOptions<InputParametersKafkaConsumer>>();

        kafkaSettingsMock.Setup(x => x.Value).Returns(new InputParametersKafkaConsumer
        {
            Consumer = new()
            {
                KafkaConsumerMappings = new List<KafkaConsumerMapping>
                {

                }
            }
        });


        // Mock do IServiceScope e IServiceScopeFactory
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

        // Configuring IServiceScopeFactory to return IServiceScope
        serviceScopeFactoryMock
            .Setup(x => x.CreateScope())
            .Returns(serviceScopeMock.Object);

        // Configuring IServiceProvider to return IServiceScopeFactory
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);

        Worker worker = new(loggerMock.Object, serviceProviderMock.Object, kafkaSettingsMock.Object);

        // Act
        await worker.StartAsync(CancellationToken.None);

        //Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains("Nenhum mapeamento de consumidor configurado.")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogInformation_WhenConsumedTopicsIsNotEmpty()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Worker>>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var kafkaSettingsMock = new Mock<IOptions<InputParametersKafkaConsumer>>();
        var consumerCollectionMock = new Mock<ConsumerCollection>();

        // Configurando o InputParametersKafkaConsumer com ConsumedTopics preenchido
        kafkaSettingsMock.Setup(x => x.Value).Returns(new InputParametersKafkaConsumer
        {
            Consumer = new()
            {
                KafkaConsumerMappings = new List<KafkaConsumerMapping>
                {
                    new KafkaConsumerMapping { Topic = "TestTopic" }
                }
            }
        });


        // Mock do IServiceScope e IServiceScopeFactory
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

        #region Configura��o de services

        // Configuring IServiceScopeFactory to return IServiceScope
        serviceScopeFactoryMock
            .Setup(x => x.CreateScope())
            .Returns(serviceScopeMock.Object);

        // Configuring IServiceProvider to return IServiceScopeFactory
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);

        // Configurando IServiceScope para retornar um IServiceProvider v�lido
        serviceScopeMock
            .Setup(x => x.ServiceProvider)
            .Returns(serviceProviderMock.Object);

        // Configurando IServiceProvider para retornar o ConsumerCollection
        serviceProviderMock
            .Setup(x => x.GetService(typeof(ConsumerCollection)))
            .Returns(consumerCollectionMock.Object);

        #endregion

        var worker = new Worker(loggerMock.Object, serviceProviderMock.Object, kafkaSettingsMock.Object);

        // Act
        await worker.StartAsync(CancellationToken.None);

        // Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Consumer iniciado")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogError_WhenExceptionIsThrown()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Worker>>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var kafkaSettingsMock = new Mock<IOptions<InputParametersKafkaConsumer>>();

        kafkaSettingsMock.Setup(x => x.Value).Returns(new InputParametersKafkaConsumer
        {
            Consumer = new()
            {
                KafkaConsumerMappings = new List<KafkaConsumerMapping>
                {
                    new KafkaConsumerMapping { Topic = "TestTopic" }
                }
            }
        });

        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

        serviceScopeFactoryMock
            .Setup(x => x.CreateScope())
            .Returns(serviceScopeMock.Object);

        serviceProviderMock
            .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);

        serviceScopeMock
            .Setup(x => x.ServiceProvider)
            .Throws(new InvalidOperationException("Erro na execu��o do Worker"));

        var worker = new Worker(loggerMock.Object, serviceProviderMock.Object, kafkaSettingsMock.Object);

        // Act
        await worker.StartAsync(CancellationToken.None);

        // Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro na execu��o do Worker")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogWarning_WhenConsumedTopicsIsNull()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Worker>>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var kafkaSettingsMock = new Mock<IOptions<InputParametersKafkaConsumer>>();

        kafkaSettingsMock.Setup(x => x.Value).Returns(new InputParametersKafkaConsumer
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
            Times.Once);
    }
}