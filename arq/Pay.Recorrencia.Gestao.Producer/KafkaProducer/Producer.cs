using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Producer.KafkaProducer.Interface;
using Pay.Recorrencia.Gestao.Producer.Models;

namespace Pay.Recorrencia.Gestao.Producer.KafkaProducer
{
    public class Producer : IDisposable, IProducer
    {
        private readonly ProducerConfig _config;
        private IProducer<Null, string> _producer;
        private ILogger<Producer> _logger;
        private readonly InputParametersKafkaProducer _inputParameterKafka;
        private readonly ConfigKafkaModel _kafkaConfigModel;

        public Producer(IOptions<InputParametersKafkaProducer> inputParameterKafka,
                        ILogger<Producer> logger)
        {
            _inputParameterKafka = inputParameterKafka.Value;
            _logger = logger;
            _kafkaConfigModel = new ConfigKafkaModel();

            var _config = ConfigKafkaProducer();

            var producer = new ProducerBuilder<Null, string>(_config);

            _producer = producer.SetLogHandler((_, error) => _logger.Log(LogLevel.Information, "ProducerBuilder: {Message}", error.Message))
                                .SetErrorHandler((_, error) => _logger.Log(LogLevel.Error, "ProducerBuilder: {Error}", error))
                                .Build();
        }

        private ProducerConfig ConfigKafkaProducer()
        {
            var saslMechanism = _inputParameterKafka.SecurityParameters?.ActivateSsl == true ? SaslMechanism.ScramSha512 : (SaslMechanism?)null;
            var securityProtocol = _inputParameterKafka.SecurityParameters?.ActivateSsl == true ? SecurityProtocol.SaslSsl : SecurityProtocol.Plaintext;

            return _kafkaConfigModel.ProduceConfigInit(
                _inputParameterKafka.BootstrapServers,
                saslMechanism ?? SaslMechanism.Plain, // Provide a default value if saslMechanism is null
                securityProtocol,
                _inputParameterKafka.SecurityParameters?.CertifiedPath ?? string.Empty,
                _inputParameterKafka.SecurityParameters?.SaslUsername ?? string.Empty,
                _inputParameterKafka.SecurityParameters?.SaslPassword ?? string.Empty
            );
        }

        public async Task ProduceAsyncMessage(Headers headers, string serializedModel, string? topic)
        {
            await ExecuteProduceAsyncMessage(
                msg: new Message<Null, string>
                {
                    Value = serializedModel,
                    Headers = headers
                },
                topic: topic,
                partition: null
            );
        }

        private async Task ExecuteProduceAsyncMessage
        (
           Message<Null, string> msg,
           string? topic,
           int? partition
        )
        {
            string DefinedTopic = topic ?? _inputParameterKafka.Producer.Topic;

            if (partition == null)
            {
                await _producer.ProduceAsync(topic, msg);
            }
            else
            {
                await _producer.ProduceAsync(
                    topicPartition: new TopicPartition(DefinedTopic, new Partition(partition.Value)),
                    message: msg
                );
            }

            _producer.Flush(TimeSpan.FromSeconds(_inputParameterKafka.Producer.ProducerFlushTime));
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
