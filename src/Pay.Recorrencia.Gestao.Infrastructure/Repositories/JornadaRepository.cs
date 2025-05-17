using System.Data;
using Dapper;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories;

    public class JornadaRepository : IJornadaRepository
    {
        private readonly IDbConnection _db;
        public JornadaRepository(IDbConnection db) => _db = db;

        public Task<IEnumerable<Jornada>> GetAllAsync()
            => _db.QueryAsync<Jornada>("SELECT * FROM dbo.Jornadas");

        public Task<Jornada> GetByTpJornadaAndIdRecorrenciaAsync(string tpJornada, string idRecorrencia)
        {
            const string sql = @"
                SELECT * FROM dbo.Jornadas
                WHERE TpJornada = @tpJornada
                  AND IdRecorrencia = @idRecorrencia";
            return _db.QueryFirstOrDefaultAsync<Jornada>(sql, new { tpJornada, idRecorrencia });
        }

        public Task<Jornada> GetByTpJornadaAndIdE2EAsync(string tpJornada, string idE2E)
        {
            const string sql = @"
                SELECT * FROM dbo.Jornadas
                WHERE TpJornada = @tpJornada
                  AND IdE2E = @idE2E";
            return _db.QueryFirstOrDefaultAsync<Jornada>(sql, new { tpJornada, idE2E });
        }

        public Task<IEnumerable<Jornada>> GetByAnyFilterAsync(
            string tpJornada,
            string idRecorrencia,
            string idE2E,
            string idConciliacaoRecebedor)
        {
            var sql = @"SELECT * FROM dbo.Jornadas WHERE 1=1";
            if (!string.IsNullOrEmpty(tpJornada))
                sql += " AND TpJornada = @tpJornada";
            if (!string.IsNullOrEmpty(idRecorrencia))
                sql += " AND IdRecorrencia = @idRecorrencia";
            if (!string.IsNullOrEmpty(idE2E))
                sql += " AND IdE2E = @idE2E";
            if (!string.IsNullOrEmpty(idConciliacaoRecebedor))
                sql += " AND IdConciliacaoRecebedor = @idConciliacaoRecebedor";

            return _db.QueryAsync<Jornada>(sql, new { tpJornada, idRecorrencia, idE2E, idConciliacaoRecebedor });
        }
    }