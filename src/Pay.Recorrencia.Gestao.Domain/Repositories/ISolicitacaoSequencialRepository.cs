namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface ISolicitacaoSequencialRepository
    {
        Task<long?> GetSequencialAsync(DateTime dataHoje);

        Task UpdateSequencialByDateAsync(DateTime dataHoje, long novoSequencial);

        Task CreateSequencialAsync(DateTime dataHoje);
    }
}
