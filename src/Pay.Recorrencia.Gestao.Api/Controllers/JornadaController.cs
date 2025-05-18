using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class JornadaController : ControllerBase
{
    private readonly IJornadaService _service;
    public JornadaController(IJornadaService service) => _service = service;

    // 1) GET /api/jornada
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    // 2) GET /api/jornada/filtro?tpJornada=…&idRecorrencia=…
    [HttpGet("filtro")]
    public async Task<IActionResult> GetByRecorrenciaAsync(
        [FromQuery][Required] string tpJornada,
        [FromQuery] string idRecorrencia)
    {
        var dto = await _service.GetByJornadaERecorrenciaAsync(tpJornada, idRecorrencia);
        if (dto == null) return NoContent();
        return Ok(dto);
    }

    // 3) GET /api/jornada/filtro-agendamento?tpJornada=…&idE2E=…
    [HttpGet("filtro-agendamento")]
    public async Task<IActionResult> GetByE2EAsync(
        [FromQuery][Required] string tpJornada,
        [FromQuery] string idE2E)
    {
        var dto = await _service.GetByJornadaE2EAsync(tpJornada, idE2E);
        if (dto == null) return NoContent();
        return Ok(dto);
    }

    // 4) GET /api/jornada/filtros?tpJornada=…[&idRecorrencia=…][&idE2E=…][&idConciliacaoRecebedor=…]
    [HttpGet("filtros")]
    public async Task<IActionResult> GetByAnyFilterAsync(
        [FromQuery][Required] string tpJornada,
        [FromQuery] string? idRecorrencia = null,
        [FromQuery] string? idE2E = null,
        [FromQuery] string? idConciliacaoRecebedor = null)
    {
        var list = await _service.GetByAnyFilterAsync(
            tpJornada,
            idRecorrencia,
            idE2E,
            idConciliacaoRecebedor);

        if (!list.Any()) return NoContent();
        return Ok(list);
    }

}