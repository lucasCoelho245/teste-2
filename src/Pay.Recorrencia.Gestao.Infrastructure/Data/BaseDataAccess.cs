using Dapper;
using DataAccess.Domain;
using Oracle.ManagedDataAccess.Client;
using Pay.Recorrencia.Gestao.Infrastructure.Data.Interfaces;
using System.Data.Common;
using System.Data.SqlClient;

namespace Pay.Recorrencia.Gestao.Infrastructure.Data
{
    public class BaseDataAccess(DBConfig dbConfig) : IBaseDataAccess
    {
        protected string _connectionString = dbConfig?.ConnectionString;
        protected string _drive = dbConfig?.Drive;

        public DbConnection CreateSession()
        {
            dynamic connection = null;
            switch (_drive)
            {
                case "Oracle":
                    connection = new OracleConnection(_connectionString);
                    break;

                case "SqlServer":
                    connection = new SqlConnection(_connectionString);
                    break;
            }

            return connection;
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            using var connection = CreateSession();
            return await connection.QueryAsync<T>(sql, param);
        }
        public async Task<int> ExecuteScalarAsync(string sql, object param = null)
        {
            using var connection = CreateSession();
            return await connection.ExecuteScalarAsync<int>(sql, param);
        }
        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using var connection = CreateSession();
            return await connection.ExecuteAsync(sql, param);
        }
    }
}