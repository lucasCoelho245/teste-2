using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories
{
    public interface IMockSolicitacaoRecorrenciaRepository
    {
        Task<SolicitacaoRecorrencia?> GetSolicitacaoRecorrencia(string id);
        Task Insert(SolicitacaoRecorrencia solicitacaoRecorrencia);
        Task Update(SolicitacaoAutorizacaoRecorrenciaUpdateDTO solicitacaoRecorrencia);
        Task<ListaSolicAutorizacaoRecPaginada> GetAllAsync(GetListaSolicAutorizacaoRecDTOPaginada data);
        Task<SolicAutorizacaoRecNonPagination> GetAsync(GetSolicAutorizacaoRecDTOPaginada data);
    }
}
