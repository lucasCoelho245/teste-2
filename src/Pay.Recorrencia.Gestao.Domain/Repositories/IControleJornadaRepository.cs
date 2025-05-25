using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface IControleJornadaRepository
    {
        Task<IList<ControleJornadaEntrada>> GetControle(string tpJornada, string idRecorrencia, string? idFimAFim);
        Task IncluirControle(ControleJornada controleJornada);
        Task AtualizarControle(ControleJornada controleJornada);
    }
}
