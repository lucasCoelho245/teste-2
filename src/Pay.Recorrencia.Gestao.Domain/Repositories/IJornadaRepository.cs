using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Domain.Repositories;

public interface IJornadaRepository
{
    Task<ListaJornadaPaginada<JornadaList>> GetAllAsync(JornadaDTO data);
    Task<JornadaNonPagination> GetByTpJornadaAndIdRecorrenciaAsync(JornadaAutorizacaoDTO data);
    Task<JornadaNonPagination?> GetByTpJornadaAndIdE2EAsync(JornadaAgendamentoDTO data);
    Task<ListaJornadaPaginada<Jornada>> GetByAnyFilterAsync(JornadaAutorizacaoAgendamentoDTO data);
}