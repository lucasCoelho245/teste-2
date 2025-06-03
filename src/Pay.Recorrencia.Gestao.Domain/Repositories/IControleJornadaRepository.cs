using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface IControleJornadaRepository
    {
        Task<IList<ControleJornada>> GetControle(string tpJornada, string idRecorrencia, string? idFimAFim);
        Task IncluirControle(ControleJornada controleJornada);
        Task AtualizarControle(ControleJornada controleJornada);
        Task<IList<ControleJornada>> BuscarPorIdRecorrenciaAsync(string idRecorrencia);

    }
}
