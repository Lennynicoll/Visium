using Microsoft.AspNetCore.Mvc;
using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Dtos.Anfitrion;

namespace RegistroVisitantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnfitrionesController : ControllerBase
    {
        private readonly IAnfitrionService _anfitrionService;

        public AnfitrionesController(IAnfitrionService anfitrionService)
        {
            _anfitrionService = anfitrionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnfitrionDTO>>> GetAnfitriones()
        {
            var anfitriones = await _anfitrionService.GetAllAsync();
            return Ok(anfitriones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnfitrionDTO>> GetAnfitrion(int id)
        {
            var anfitrion = await _anfitrionService.GetByIdAsync(id);
            if (anfitrion == null)
                return NotFound();

            return Ok(anfitrion);
        }

        [HttpPost]
        public async Task<ActionResult<AnfitrionDTO>> PostAnfitrion(AnfitrionDTO anfitrionDto)
        {
            var result = await _anfitrionService.CreateWithValidationAsync(anfitrionDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return CreatedAtAction(nameof(GetAnfitrion), new { id = ((AnfitrionDTO)result.Data!).Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnfitrion(int id, AnfitrionDTO anfitrionDto)
        {
            var result = await _anfitrionService.UpdateWithValidationAsync(id, anfitrionDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnfitrion(int id)
        {
            var deleted = await _anfitrionService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("MotivoVisita/{motivoVisitaId}")]
        public async Task<ActionResult<IEnumerable<AnfitrionDTO>>> GetByMotivoVisitaId(int motivoVisitaId)
        {
            var anfitriones = await _anfitrionService.GetByMotivoVisitaIdAsync(motivoVisitaId);
            return Ok(anfitriones);
        }
    }
}

