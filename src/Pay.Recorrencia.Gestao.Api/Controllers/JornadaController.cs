using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Services;

namespace Pay.Recorrencia.Gestao.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JornadaController : ControllerBase
{
    private readonly IJornadaService _service;
    public JornadaController(IJornadaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [HttpGet("filtro")]
    public async Task<IActionResult> GetByRecorrencia(
        [FromQuery] string tpJornada,
        [FromQuery] string idRecorrencia)
    {
        if (string.IsNullOrEmpty(tpJornada) || string.IsNullOrEmpty(idRecorrencia))
            return BadRequest(new { tpJornada = "required", idRecorrencia = "required" });

        var dto = await _service.GetByJornadaERecorrenciaAsync(tpJornada, idRecorrencia);
        return Ok(dto);
    }

    [HttpGet("filtro-agendamento")]
    public async Task<IActionResult> GetByAgendamento(
        [FromQuery] string tpJornada,
        [FromQuery] string idE2E)
    {
        if (string.IsNullOrEmpty(tpJornada) || string.IsNullOrEmpty(idE2E))
            return BadRequest(new { tpJornada = "required", idE2E = "required" });

        var dto = await _service.GetByJornadaE2EAsync(tpJornada, idE2E);
        return Ok(dto);
    }

    [HttpGet("filtros")]
    public async Task<IActionResult> GetByFiltros(
        [FromQuery] string tpJornada,
        [FromQuery] string idRecorrencia,
        [FromQuery] string idE2E,
        [FromQuery] string idConciliacaoRecebedor)
    {
        var list = await _service.GetByAnyFilterAsync(tpJornada, idRecorrencia, idE2E, idConciliacaoRecebedor);
        return Ok(list);
    }
}