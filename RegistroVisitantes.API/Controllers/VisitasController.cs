using Microsoft.AspNetCore.Mvc;
using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Dtos.Visita;

namespace RegistroVisitantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitasController : ControllerBase
    {
        private readonly IVisitaService _visitaService;

        public VisitasController(IVisitaService visitaService)
        {
            _visitaService = visitaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitaDTO>>> GetVisitas()
        {
            var visitas = await _visitaService.GetAllAsync();
            return Ok(visitas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitaDTO>> GetVisita(int id)
        {
            var visita = await _visitaService.GetByIdAsync(id);
            if (visita == null)
                return NotFound();

            return Ok(visita);
        }

        [HttpPost]
        public async Task<ActionResult<VisitaDTO>> PostVisita(VisitaDTO visitaDto)
        {
            var result = await _visitaService.CreateWithValidationAsync(visitaDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return CreatedAtAction(nameof(GetVisita), new { id = ((VisitaDTO)result.Data!).Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisita(int id, VisitaDTO visitaDto)
        {
            var result = await _visitaService.UpdateWithValidationAsync(id, visitaDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisita(int id)
        {
            var deleted = await _visitaService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("visitante/{visitanteId}")]
        public async Task<ActionResult<IEnumerable<VisitaDTO>>> GetByVisitanteId(int visitanteId)
        {
            var visitas = await _visitaService.GetByVisitanteIdAsync(visitanteId);
            return Ok(visitas);
        }

        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<IEnumerable<VisitaDTO>>> GetByEstado(string estado)
        {
            var visitas = await _visitaService.GetByEstadoAsync(estado);
            return Ok(visitas);
        }

        [HttpPost("{id}/entrada")]
        public async Task<ActionResult<VisitaDTO>> RegistrarEntrada(int id)
        {
            var result = await _visitaService.RegistrarEntradaAsync(id);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return Ok(result.Data);
        }

        [HttpPost("{id}/salida")]
        public async Task<ActionResult<VisitaDTO>> RegistrarSalida(int id)
        {
            var result = await _visitaService.RegistrarSalidaAsync(id);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return Ok(result.Data);
        }
    }
}

