using Pay.Recorrencia.Gestao.Domain.DTO;

namespace Pay.Recorrencia.Gestao.Application.Services;

public interface IJornadaService
{
    Task<IEnumerable<JornadaDto>> GetAllAsync();
    Task<JornadaDto> GetByJornadaERecorrenciaAsync(string tpJornada, string idRecorrencia);
    Task<JornadaDto> GetByJornadaE2EAsync(string tpJornada, string idE2E);
    Task<IEnumerable<JornadaDto>> GetByAnyFilterAsync(
        string tpJornada,
        string idRecorrencia,
        string idE2E,
        string idConciliacaoRecebedor
    );
}