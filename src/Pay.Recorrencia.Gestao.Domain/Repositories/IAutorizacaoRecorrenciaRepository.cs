using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface IAutorizacaoRecorrenciaRepository
    {
        #region IncluirAutorizacaoRecorrencia
        Task<AutorizacaoRecorrencia> ConsultaAutorizacao(string? IdAutorizacao, string? IdRecorrencia);
        Task<AtualizacaoAutorizacaoRecorrencia> ConsultarAtualizacaoAutorizacaoRecorrencia(string? IdAutorizacao, string? IdRecorrencia);
        Task<AutorizacaoRecorrencia> ConsultaAutorizacao(string IdRecorrencia);
        Task<AtualizacaoAutorizacaoRecorrencia> ConsultarAtualizacaoAutorizacaoRecorrencia(string IdRecorrencia);


        Task<AutorizacaoRecorrencia> InsertAutorizacaoRecorrencia(AutorizacaoRecorrencia autorizacaoRecorrencia);

        Task<AtualizacaoAutorizacaoRecorrencia> InsertAtualizacoesAutorizacaoRecorrencia(AutorizacaoRecorrencia atualizacaoAutorizacaoRecorrencia);

        Task<AtualizacaoAutorizacaoRecorrencia> InsertAtualizacoesAutorizacaoRecorrencia(AtualizacaoAutorizacaoRecorrencia atualizacaoAutorizacao);

        #endregion


        #region AlterarAutorizacaoRecorrencia
        Task<IEnumerable<AtualizacaoAutorizacaoRecorrencia>> ObterAtualizacoesAsync();
        Task<AtualizacaoAutorizacaoRecorrencia> ObterAtualizacoesPorIdAsync(Guid id);

        void Update(AutorizacaoRecorrencia autorizacao);
        #endregion
        Task<ListaAutorizacaoRecPaginada> GetAllAsync(GetListaAutorizacaoRecDTOPaginada data);
        Task<AutorizacaoRecNonPagination> GetAsync(GetAutorizacaoRecDTOPaginada data);
        string ConsultarCodMunIBGE();
    }
}
