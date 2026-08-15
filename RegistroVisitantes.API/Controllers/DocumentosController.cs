using Microsoft.AspNetCore.Mvc;
using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Dtos.Documento;

namespace RegistroVisitantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentoService _documentoService;

        public DocumentosController(IDocumentoService documentoService)
        {
            _documentoService = documentoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentoDTO>>> GetDocumentos()
        {
            var documentos = await _documentoService.GetAllAsync();
            return Ok(documentos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentoDTO>> GetDocumento(int id)
        {
            var documento = await _documentoService.GetByIdAsync(id);
            if (documento == null)
                return NotFound();

            return Ok(documento);
        }

        [HttpPost]
        public async Task<ActionResult<DocumentoDTO>> PostDocumento(DocumentoDTO documentoDto)
        {
            var result = await _documentoService.CreateWithValidationAsync(documentoDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return CreatedAtAction(nameof(GetDocumento), new { id = ((DocumentoDTO)result.Data!).Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumento(int id, DocumentoDTO documentoDto)
        {
            var result = await _documentoService.UpdateWithValidationAsync(id, documentoDto);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var deleted = await _documentoService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("visitante/{visitanteId}")]
        public async Task<ActionResult<IEnumerable<DocumentoDTO>>> GetByVisitante(int visitanteId)
        {
            var documentos = await _documentoService.GetByVisitanteIdAsync(visitanteId);
            return Ok(documentos);
        }
    }
}

