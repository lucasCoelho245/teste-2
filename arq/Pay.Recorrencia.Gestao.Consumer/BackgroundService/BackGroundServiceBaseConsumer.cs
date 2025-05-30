﻿using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Consumer.KafkaConsumer;
using Pay.Recorrencia.Gestao.Consumer.Models;

namespace Pay.Recorrencia.Gestao.Consumer.BackGroundService
{
    public abstract class BackGroundServiceBaseConsumer(
        IOptions<InputParametersKafkaConsumer> kafkaparameter,
        ILogger<BackGroundServiceBaseConsumer> logger,
        ConsumerServices consumerServices,
        IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly ILogger<BackGroundServiceBaseConsumer> _logger = logger;
        protected readonly InputParametersKafkaConsumer _inputKafkaParameter = kafkaparameter.Value;
        private readonly ConsumerServices consumerServices = consumerServices;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topics = GetTopics();
            if (topics == null || topics.Length == 0)
            {
                _logger.LogWarning("ConsumedTopics is null or empty.");
                return;
            }

            _logger.LogInformation("Consumer Hosted Service is listening: {ConsumedTopics}.", topics);
            await DoWork(stoppingToken);
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            bool continueWorking = true;

            while (!stoppingToken.IsCancellationRequested && continueWorking)
            {
                var _KafkaConsumer = scope.ServiceProvider.GetRequiredService<KafkaConsumer.Consumer>();
                using var consumidor = _KafkaConsumer.GetKafkaConsumer();
                var cts = new CancellationTokenSource();

                try
                {
                    await ConsumeMessageKafka(consumidor, cts);
                }
                catch (OperationCanceledException x)
                {
                    consumidor.Close();
                    _logger.LogError(x, "Consumidor cancelado. Ex: {Message}", x.Message);
                    continueWorking = false; // Break out of the loop
                }
                catch (Exception x)
                {
                    consumidor.Close();
                    _logger.LogError(x, "Falha no consumidor: {Message} - {InnerExceptionMessage}", x.Message, x.InnerException?.Message);
                    continueWorking = false; // Break out of the loop
                }
            }
        }

        private async Task ConsumeMessageKafka(IConsumer<Null, string> consumidor, CancellationTokenSource cts, int maxRetries = 5)
        {
            int retryCount = 0;

            while (!cts.Token.IsCancellationRequested && retryCount < maxRetries)
            {
                var result = await ConsumeMessage(consumidor, cts.Token);
                if (result == null)
                {
                    _logger.LogWarning("Message discarded because it is not the message of the Consumer Group.");
                    retryCount++;
                    continue;
                }

                /// Validar headers especificos?

                await OrchestrateMessage(result, result.Topic);

                var commitResult = consumidor.Commit();
                _logger.LogDebug("Total Commitado: {Count}", commitResult.Count);
                retryCount = 0; // Reset retry count after a successful operation
            }

            if (retryCount >= maxRetries)
            {
                _logger.LogError("Maximum retry count reached. Exiting ConsumeMessageKafka.");
            }
        }

        protected abstract Task<ConsumeResult<Null, string>> ConsumeMessage(IConsumer<Null, string> consumidor, CancellationToken token);

        private async Task OrchestrateMessage(ConsumeResult<Null, string> result, string topic)
        {
            await Task.Run(() =>
            {
                consumerServices.Consume(result, topic);
            }, new CancellationToken());
        }

        private string?[] GetTopics()
        {
            return _inputKafkaParameter?.Consumer?.KafkaConsumerMappings?
                        .Select(static m => m.Topic)
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .ToArray() ?? [];
        }
    }
}
