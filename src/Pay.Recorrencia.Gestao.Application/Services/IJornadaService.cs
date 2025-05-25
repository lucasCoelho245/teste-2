using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;

namespace Pay.Recorrencia.Gestao.Application.Services;

public interface IJornadaService
{
    //Task<ListaJornadaPaginada> GetAllAsync(GetListaJornadaDTOPaginada request);
    //Task<JornadaDto> GetByJornadaERecorrenciaAsync(string tpJornada, string idRecorrencia);
    //Task<JornadaDto> GetByJornadaE2EAsync(string tpJornada, string idE2E);
    //Task<IEnumerable<JornadaDto>> GetByAnyFilterAsync(
    //    string tpJornada,
    //    string idRecorrencia,
    //    string idE2E,
    //    string idConciliacaoRecebedor
    //);

    Task<JornadaNonPagination> GetByTpJornadaAndIdRecorrenciaAsync(JornadaDTO data);
    Task<JornadaNonPagination> GetByTpJornadaAndIdE2EAsync(JornadaDTO data);
    Task<JornadaNonPagination> GetByAnyFilterAsync(JornadaDTO data);
}