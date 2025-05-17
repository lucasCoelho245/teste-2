using Pay.Recorrencia.Gestao.Domain.DTO;
using Pay.Recorrencia.Gestao.Domain.Entities;
using Pay.Recorrencia.Gestao.Domain.Repositories;

namespace Pay.Recorrencia.Gestao.Application.Services;

public class JornadaService
{
    private readonly IJornadaRepository _repo;
    public JornadaService(IJornadaRepository repo) => _repo = repo;

    public async Task<IEnumerable<JornadaDto>> GetAllAsync()
    {
        var list = await _repo.GetAllAsync();
        return list.Select(ToDto);
    }

    public async Task<JornadaDto> GetByJornadaERecorrenciaAsync(string tpJornada, string idRecorrencia)
    {
        var j = await _repo.GetByTpJornadaAndIdRecorrenciaAsync(tpJornada, idRecorrencia);
        return j == null ? null : ToDto(j);
    }

    public async Task<JornadaDto> GetByJornadaE2EAsync(string tpJornada, string idE2E)
    {
        var j = await _repo.GetByTpJornadaAndIdE2EAsync(tpJornada, idE2E);
        return j == null ? null : ToDto(j);
    }

    public async Task<IEnumerable<JornadaDto>> GetByAnyFilterAsync(
        string tpJornada,
        string idRecorrencia,
        string idE2E,
        string idConciliacaoRecebedor)
    {
        var list = await _repo.GetByAnyFilterAsync(
            tpJornada, idRecorrencia, idE2E, idConciliacaoRecebedor);
        return list.Select(ToDto);
    }

    private static JornadaDto ToDto(Jornada j) => new JornadaDto
    {
        Id                     = j.Id,
        TpJornada              = j.TpJornada,
        IdRecorrencia          = j.IdRecorrencia,
        IdE2E                  = j.IdE2E,
        IdConciliacaoRecebedor = j.IdConciliacaoRecebedor,
        SituacaoJornada        = j.SituacaoJornada,
        DtAgendamento          = j.DtAgendamento,
        VlAgendamento          = j.VlAgendamento,
        DtPagamento            = j.DtPagamento,
        DataHoraCriacao        = j.DataHoraCriacao,
        DataUltimaAtualizacao  = j.DataUltimaAtualizacao
    };
}