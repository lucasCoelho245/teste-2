using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface IMessageDataBaseRepository
    {
        Task InsertAsync(MessageDatabase messageDatabase);
    }
}
