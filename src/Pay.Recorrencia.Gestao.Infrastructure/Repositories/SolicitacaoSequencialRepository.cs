using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Infrastructure.Data.Interfaces;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class SolicitacaoSequencialRepository : ISolicitacaoSequencialRepository
    {
        private IDataAccess _dataAccess { get; }
        private readonly IPixAutomaticoDataAccess _dbContext;

        public SolicitacaoSequencialRepository(IDataAccess db, IPixAutomaticoDataAccess dbContext)
        {
            _dataAccess = db;
            _dbContext = dbContext;
        }

        public async Task<long?> GetSequencialAsync(DateTime dataHoje)
        {           
            const string query = @"SELECT sequencial 
                                        FROM SOLICITACAO_SEQUENCIAL
                                        WHERE data = @Data";

            using var session = _dataAccess.CreateSession();
            try
            {
                session.Begin();

                var result = await session.QueryAsync<long?>(query, new { Data = dataHoje });

                return result.FirstOrDefault();
            }
            catch
            {
                session.Rollback();
                throw;
            }

        }

        public async Task UpdateSequencialByDateAsync(DateTime dataHoje, long novoSequencial)
        {
            const string query = @"UPDATE SOLICITACAO_SEQUENCIAL 
                                    SET sequencial = @NovoSequencial
                                    WHERE data = @Data";

            using var session = _dataAccess.CreateSession();
            try
            {
                session.Begin();

                session.Execute(query, new { Data = dataHoje, NovoSequencial = novoSequencial });

                session.Commit();

                await Task.CompletedTask;
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public async Task CreateSequencialAsync(DateTime dataHoje)
        {
            const string query = @"INSERT INTO SOLICITACAO_SEQUENCIAL (data, sequencial) 
                                    VALUES (@Data, 0)";

            using var session = _dataAccess.CreateSession();
            try
            {
                session.Begin();

                session.Execute(query, new { Data = dataHoje });

                session.Commit();

                await Task.CompletedTask;
            }
            catch
            {
                session.Rollback();
                throw;
            }

        }
    }
}
