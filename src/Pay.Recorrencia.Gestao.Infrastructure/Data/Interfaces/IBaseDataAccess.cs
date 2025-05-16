namespace Pay.Recorrencia.Gestao.Infrastructure.Data.Interfaces
{
    public interface IBaseDataAccess
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param);
        Task<int> ExecuteScalarAsync(string sql, object param);
        Task<int> ExecuteAsync(string sql, object param = null);
    }
}