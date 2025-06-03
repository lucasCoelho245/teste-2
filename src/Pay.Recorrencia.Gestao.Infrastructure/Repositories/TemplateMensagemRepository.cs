using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using Pay.Recorrencia.Gestao.Infrastructure.Data.Interfaces;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class TemplateMensagemRepository : ITemplateMensagemRepository
    {
        private IDataAccess Db { get; }
        private readonly IPixAutomaticoDataAccess _dbContext;
        public TemplateMensagemRepository(IDataAccess db, IPixAutomaticoDataAccess dbContext)
        {
            Db = db;
            _dbContext = dbContext;
        }

        public async Task<TemplateMensagem> GetTemplateMensagem(string idMensagem)
        {
            const string query = @"SELECT idMensagem, txTemplate, dataUltimaAtualizacao 
                                FROM MODELOS_MENSAGEM
                                WHERE idMensagem = @IdMensagem";

            using (var session = this.Db.CreateSession())
            {
                try
                {
                    session.Begin();

                    var result = await session.QueryAsync<TemplateMensagem>(query, new { IdMensagem = idMensagem });

                    return result.FirstOrDefault();
                }
                catch
                {
                    session.Rollback();
                    throw;
                }
            }
        }

        public async Task Insert(TemplateMensagem mensagem)
        {
            using var session = Db.CreateSession();
            session.Begin();
            try
            {
                mensagem.dataUltimaAtualizacao = DateTime.UtcNow;
                string query = @" INSERT INTO [dbo].[MODELOS_MENSAGEM] 
                                 (idMensagem,
                                  txTemplate,
                                  dataUltimaAtualizacao)
                            VALUES
                                 (@idMensagem,
                                  @txTemplate,
                                  @dataUltimaAtualizacao)";

                session.Execute(query, mensagem);
                session.Commit();

                await Task.CompletedTask;
            }
            catch (Exception)
            {
                session.Rollback();
                throw;
            }
              


            
        }
    }
}
