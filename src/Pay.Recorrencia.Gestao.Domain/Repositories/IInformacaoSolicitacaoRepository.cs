namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface IInformacaoSolicitacaoRepository
    {
        Task<long> ObterENovoSequencialAsync(string ispb);
    }
}
