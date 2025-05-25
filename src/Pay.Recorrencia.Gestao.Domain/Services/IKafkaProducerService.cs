using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Services
{
    public interface IKafkaProducerService
    {
        Task SendMessageAsync(string topic, string message);

        Task SendObjectAsync<T>(string topic, T obj);

        Task EnviarEventoAsync(AtualizarControleDePedidos evento, string topic);

        Task SendMessageWithParameterAsync(string topic, string message);

    }
}