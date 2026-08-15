using Microsoft.AspNetCore.Mvc;
using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Dtos.SeguridadEdificio;

namespace RegistroVisitantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadEdificioController : ControllerBase
    {
        private readonly ISeguridadEdificioService _seguridadEdificioService;

        public SeguridadEdificioController(ISeguridadEdificioService seguridadEdificioService)
        {
            _seguridadEdificioService = seguridadEdificioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeguridadEdificioDTO>>> GetSeguridad()
        {
            var seguridad = await _seguridadEdificioService.GetAllAsync();
            return Ok(seguridad);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeguridadEdificioDTO>> GetRegistroSeguridad(int id)
        {
            var registro = await _seguridadEdificioService.GetByIdAsync(id);
            if (registro == null)
                return NotFound();

            return Ok(registro);
        }

        [HttpPost]
        public async Task<ActionResult<SeguridadEdificioDTO>> PostSeguridad(SeguridadEdificioDTO seguridadDto)
        {
            var result = await _seguridadEdificioService.CreateWithValidationAsync(seguridadDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return CreatedAtAction(nameof(GetRegistroSeguridad), new { id = ((SeguridadEdificioDTO)result.Data!).Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeguridad(int id, SeguridadEdificioDTO seguridadDto)
        {
            var result = await _seguridadEdificioService.UpdateWithValidationAsync(id, seguridadDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguridad(int id)
        {
            var deleted = await _seguridadEdificioService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}

