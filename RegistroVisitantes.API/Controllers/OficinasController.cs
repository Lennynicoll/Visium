using Microsoft.AspNetCore.Mvc;
using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Dtos.Oficina;

namespace RegistroVisitantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OficinasController : ControllerBase
    {
        private readonly IOficinaService _oficinaService;

        public OficinasController(IOficinaService oficinaService)
        {
            _oficinaService = oficinaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OficinaDTO>>> GetOficinas()
        {
            var oficinas = await _oficinaService.GetAllAsync();
            return Ok(oficinas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OficinaDTO>> GetOficina(int id)
        {
            var oficina = await _oficinaService.GetByIdAsync(id);
            if (oficina == null)
                return NotFound();

            return Ok(oficina);
        }

        [HttpPost]
        public async Task<ActionResult<OficinaDTO>> PostOficina(OficinaDTO oficinaDto)
        {
            var result = await _oficinaService.CreateWithValidationAsync(oficinaDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return CreatedAtAction(nameof(GetOficina), new { id = ((OficinaDTO)result.Data!).Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOficina(int id, OficinaDTO oficinaDto)
        {
            var result = await _oficinaService.UpdateWithValidationAsync(id, oficinaDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOficina(int id)
        {
            var deleted = await _oficinaService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<IEnumerable<OficinaDTO>>> SearchByName(string nombre)
        {
            var oficinas = await _oficinaService.SearchByNameAsync(nombre);
            return Ok(oficinas);
        }
    }
}

