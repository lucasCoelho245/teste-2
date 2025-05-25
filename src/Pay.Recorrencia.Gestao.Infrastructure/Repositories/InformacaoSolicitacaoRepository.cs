using DataAccess.Domain;
using Pay.Recorrencia.Gestao.Domain.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
{
    public class InformacaoSolicitacaoRepository(IDataAccess dataAccess) : IInformacaoSolicitacaoRepository
    {
        private readonly IDataAccess _dataAccess = dataAccess;

        public async Task<long> ObterENovoSequencialAsync(string ispb)
        {
            const string selectSql = "SELECT ISNULL(MAX(SEQUENCIAL), 0) FROM INFORMACAO_SOLICITACAO_CONTROLE WHERE DATA = @Data";
            const string insertSql = "INSERT INTO INFORMACAO_SOLICITACAO_CONTROLE (ISPB, DATA, SEQUENCIAL) VALUES (@ISPB, @Data, @NumeroSequencial)";

            var dataHoje = DateTime.UtcNow.Date;
            long novoSequencial;

            using var session = _dataAccess.CreateSession();
            try
            {
                
                session.Begin();

                var result = await session.QueryAsync<int>(selectSql, new { Data = dataHoje });
                var sequencialAtual = result.FirstOrDefault();
                novoSequencial = sequencialAtual + 1;

                session.Execute(insertSql, new { ISPB = ispb, Data = dataHoje, NumeroSequencial = novoSequencial });
                session.Commit();

                return novoSequencial;
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }
    }
}
