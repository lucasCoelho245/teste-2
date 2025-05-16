using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface IAutorizacaoRecorrenciaRepository
    {
        #region IncluirAutorizacaoRecorrencia

        Task<AutorizacaoRecorrencia> ConsultaAutorizacao(Guid id);

        Task<AtualizacaoAutorizacaoRecorrencia> ConsultarAtualizacaoAutorizacaoRecorrencia(Guid id);

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
        Task<AutorizacaoRecPagination> GetAsync(GetAutorizacaoRecDTOPaginada data);
    }
}
