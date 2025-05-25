using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Services;
using Pay.Recorrencia.Gestao.Producer.KafkaProducer.Interface;
using Pay.Recorrencia.Gestao.Producer.Models;

namespace Pay.Recorrencia.Gestao.Infrastructure.Services
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private ILogger<KafkaProducerService> _logger;
        private readonly IProducer _producer;
        private InputParametersKafkaProducer _config;

        public KafkaProducerService(IOptions<InputParametersKafkaProducer> options, ILogger<KafkaProducerService> logger, IProducer producer)
        {
            _logger = logger;
            _producer = producer;
            _config = options.Value;
        }

        public async Task EnviarEventoAsync(AtualizarControleDePedidos evento, string topic)
        {
            try
            {
                var headers = new Headers();
                // Adicione cabeçalhos conforme necessário, por exemplo:
                headers.Add("key", new byte[] { 1, 2, 3 });

                await _producer.EnviarEventoAsync(evento, topic);
                _logger.LogInformation($"Mensagem enviada");
            }
            catch (Exception e)
            {
                _logger.LogError($"Falha ao enviar mensagem: {e.Message}");
            }
        }

        public async Task SendMessageAsync(string topic, string message)
        {
            try
            {
                var headers = new Headers();
                // Adicione cabeçalhos conforme necessário, por exemplo:
                headers.Add("key", new byte[] { 1, 2, 3 });

                await _producer.ProduceAsyncMessage(headers, message, topic);
                _logger.LogInformation($"Mensagem enviada");
            }
            catch (Exception e)
            {
                _logger.LogError($"Falha ao enviar mensagem: {e.Message}");
            }
        }

        public async Task SendMessageWithParameterAsync(string topic, string message)
        {

            try
            {
                var headers = new Headers();
                // Adicione cabeçalhos conforme necessário, por exemplo:
                headers.Add("key", new byte[] { 1, 2, 3 });

                await _producer.ProduceAsyncMessage(headers, message, topic);
                _logger.LogInformation($"Mensagem enviada");
            }
            catch (Exception e)
            {
                _logger.LogError($"Falha ao enviar mensagem: {e.Message}");
            }
        }

        public async Task SendObjectAsync<T>(string topic, T obj)
        {
            try
            {
                var headers = new Headers();
                // Adicione cabeçalhos conforme necessário, por exemplo:
                headers.Add("key", new byte[] { 1, 2, 3 });

                var jsonMessage = JsonSerializer.Serialize(obj);
                await _producer.ProduceAsyncMessage(headers, jsonMessage, topic);
            }
            catch (Exception e)
            {
                _logger.LogError($"Falha ao enviar mensagem: {e.Message}");
            }
        }
    }
}