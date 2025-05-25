using Pay.Recorrencia.Gestao.Domain.Events;

namespace Pay.Recorrencia.Gestao.Application.Events
{
    public interface IControleJornadaPublisher
    {
        Task PublicarAsync(ControleJornadaEvent evento);
    }
}
