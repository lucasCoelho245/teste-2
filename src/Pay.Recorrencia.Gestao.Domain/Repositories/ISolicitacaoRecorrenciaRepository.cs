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
        Task<SolicAutorizacaoRecPagination> GetAsync(GetSolicAutorizacaoRecDTOPaginada data);
    }
}
