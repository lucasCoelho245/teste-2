using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Consumer.Models;

namespace Pay.Recorrencia.Gestao.Infrastructure.Services;
public class KafkaConsumerService(IOptions<InputParametersKafkaConsumer> config, ILogger<KafkaConsumerService> logger) : IHostedService
{
    private readonly ILogger<KafkaConsumerService> _logger = logger;
    private readonly InputParametersKafkaConsumer _config = config.Value;
    private IConsumer<Ignore, string>? _consumer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _config.BootstrapServers,
            GroupId = _config?.Consumer?.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false // Desabilitar auto commit para controle manual
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _consumer.Subscribe(_config?.Consumer?.KafkaConsumerMappings?.Select(x => x.Topic));

        Task.Run(() => ConsumeMessages(cancellationToken), cancellationToken);

        return Task.CompletedTask;
    }

    private void ConsumeMessages(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer?.Consume(cancellationToken);
                if (consumeResult != null)
                {
                    _logger.LogInformation("Message: {MessageValue}", consumeResult.Message.Value);

                    // Processar a mensagem aqui

                    // Commit manual do offset após processar a mensagem
                    _consumer?.Commit(consumeResult);
                    _logger.LogInformation("Offset committed: {Offset}", consumeResult.Offset);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Consuming messages was canceled.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error consuming messages: {ErrorMessage}", ex.Message);
        }
        finally
        {
            _consumer?.Close();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer?.Close();
        return Task.CompletedTask;
    }
}
