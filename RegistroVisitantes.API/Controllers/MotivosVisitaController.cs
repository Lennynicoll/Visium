using Microsoft.AspNetCore.Mvc;
using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Dtos.MotivoVisita;

namespace RegistroVisitantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivosVisitaController : ControllerBase
    {
        private readonly IMotivoVisitaService _motivoVisitaService;

        public MotivosVisitaController(IMotivoVisitaService motivoVisitaService)
        {
            _motivoVisitaService = motivoVisitaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotivoVisitaDTO>>> GetMotivos()
        {
            var motivos = await _motivoVisitaService.GetAllAsync();
            return Ok(motivos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MotivoVisitaDTO>> GetMotivo(int id)
        {
            var motivo = await _motivoVisitaService.GetByIdAsync(id);
            if (motivo == null)
                return NotFound();

            return Ok(motivo);
        }

        [HttpPost]
        public async Task<ActionResult<MotivoVisitaDTO>> PostMotivo(MotivoVisitaDTO motivoDto)
        {
            var result = await _motivoVisitaService.CreateWithValidationAsync(motivoDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return CreatedAtAction(nameof(GetMotivo), new { id = ((MotivoVisitaDTO)result.Data!).Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMotivo(int id, MotivoVisitaDTO motivoDto)
        {
            var result = await _motivoVisitaService.UpdateWithValidationAsync(id, motivoDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotivo(int id)
        {
            var deleted = await _motivoVisitaService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}

