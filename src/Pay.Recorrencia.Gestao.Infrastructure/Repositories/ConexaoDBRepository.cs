//using System.Configuration;
//using Dapper;
//using Microsoft.Data.Sqlite;
//using Microsoft.Extensions.Configuration;
//using Pay.Recorrencia.Gestao.Domain.Entities;
//using Pay.Recorrencia.Gestao.Domain.Repositories;

//namespace Pay.Recorrencia.Gestao.Infrastructure.Repositories
//{
//    public class ConexaoDBRepository : IConexaoDBRepository
//    {
//        private readonly string _connectionString;

//        public ConexaoDBRepository()
//        {
//            var dbPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "base.db"));
//            _connectionString = $"Data Source={dbPath}";
//            CriarTabelaSeNaoExistir();
//        }

//        private void CriarTabelaSeNaoExistir()
//        {
//            using var connection = new SqliteConnection(_connectionString);
//            connection.Execute(
//                """
//                    CREATE TABLE IF NOT EXISTS Produtos (
//                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                        Nome TEXT NOT NULL,
//                        Preco REAL NOT NULL
//                    );
//                """
//            );
//        }

//        public async Task InserirAsync(ConexaoDB produto)
//        {
//            using var connection = new SqliteConnection(_connectionString);
//            await connection.ExecuteAsync(
//                "INSERT INTO Produtos (Nome, Preco) VALUES (@Nome, @Preco)", produto);
//        }

//        public async Task<IEnumerable<GetConexaoDB>> GetAllAsync()
//        {
//            IEnumerable<GetConexaoDB> result = null;
//            using var connection = new SqliteConnection(_connectionString);
//            var result = await connection.QueryAsync<GetConexaoDB>("SELECT * FROM Produtos");
//            return result;
//        }

//    }
//}
