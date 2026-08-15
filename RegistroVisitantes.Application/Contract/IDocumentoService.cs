using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.Documento;

namespace RegistroVisitantes.Application.Contract
{
    public interface IDocumentoService : IBaseService<DocumentoDTO>
    {
        Task<ServiceResult> CreateWithValidationAsync(DocumentoDTO dto);
        Task<ServiceResult> UpdateWithValidationAsync(int id, DocumentoDTO dto);
        Task<IEnumerable<DocumentoDTO>> GetByVisitanteIdAsync(int visitanteId);
    }
}
