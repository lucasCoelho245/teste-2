using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface ISolicitacaoRecorrenciaRepository
    {
        Task<SolicitacaoRecorrencia?> GetSolicitacaoRecorrencia(string id);
        Task Insert(SolicitacaoRecorrencia solicitacaoRecorrencia);
        Task Update(SolicitacaoAutorizacaoRecorrenciaUpdateDTO solicitacaoRecorrencia);
        Task<ListaSolicAutorizacaoRecPaginada> GetAllAsync(GetListaSolicAutorizacaoRecDTOPaginada data);
        Task<SolicitacaoRecorrencia> GetById(string id);
        string ConsultarCodMunIBGE();
        Task<SolicitacaoRecorrencia> InsertSolicitacaoRecorrencia(SolicitacaoRecorrencia solicitacaoRecorrencia);
        Task<bool> ExistsByIdAsync(string id);
        Task InsertAsync(SolicitacaoRecorrencia solicitacaoRecorrencia);
        Task<SolicAutorizacaoRecNonPagination> GetAsync(GetSolicAutorizacaoRecDTOPaginada data);
    }
}
