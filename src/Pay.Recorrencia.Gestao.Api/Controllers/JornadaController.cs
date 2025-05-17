using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pay.Recorrencia.Gestao.Application.Services;

namespace Pay.Recorrencia.Gestao.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JornadaController : ControllerBase
    {
        private readonly JornadaService _service;
        public JornadaController(JornadaService service)
            => _service = service;

        // Cenário 01 & 03.2: retorna todas as jornadas
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        // Cenário 02.1 & 03.1: filtra por tpJornada + idRecorrencia
        [HttpGet("filtro")]
        public async Task<IActionResult> GetByJornadaERecorrencia(
            [FromQuery] string tpJornada,
            [FromQuery] string idRecorrencia)
        {
            var dto = await _service.GetByJornadaERecorrenciaAsync(tpJornada, idRecorrencia);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // Cenário 02.2 & 03.1: filtra por tpJornada + idE2E
        [HttpGet("filtro-agendamento")]
        public async Task<IActionResult> GetByJornadaE2E(
            [FromQuery] string tpJornada,
            [FromQuery] string idE2E)
        {
            var dto = await _service.GetByJornadaE2EAsync(tpJornada, idE2E);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // Cenário 02.3 & 03.2: demais combinações
        [HttpGet("filtros")]
        public async Task<IActionResult> GetByAny(
            [FromQuery] string tpJornada = null,
            [FromQuery] string idRecorrencia = null,
            [FromQuery] string idE2E = null,
            [FromQuery] string idConciliacaoRecebedor = null)
            => Ok(await _service.GetByAnyFilterAsync(
                tpJornada, idRecorrencia, idE2E, idConciliacaoRecebedor));
    }
}