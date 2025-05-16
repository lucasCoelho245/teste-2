namespace Pay.Recorrencia.Gestao.Domain.Services
{
    public interface IKafkaConsumerService
    {
        Task StartConsumeAsync(Func<string, string, CancellationToken, Task> callBack, string topic, CancellationToken cancellationToken);
    }
}
