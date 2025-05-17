using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories;

public interface IJornadaRepository
{
    Task<IEnumerable<Jornada>> GetAllAsync();
    Task<Jornada> GetByTpJornadaAndIdRecorrenciaAsync(string tpJornada, string idRecorrencia);
    Task<Jornada> GetByTpJornadaAndIdE2EAsync(string tpJornada, string idE2E);

    Task<IEnumerable<Jornada>> GetByAnyFilterAsync(
        string tpJornada,
        string idRecorrencia,
        string idE2E,
        string idConciliacaoRecebedor);
}