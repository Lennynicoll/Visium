using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.Visita;

namespace RegistroVisitantes.Application.Contract
{
    public interface IVisitaService : IBaseService<VisitaDTO>
    {
        Task<ServiceResult> CreateWithValidationAsync(VisitaDTO dto);
        Task<ServiceResult> UpdateWithValidationAsync(int id, VisitaDTO dto);
        Task<ServiceResult> RegistrarEntradaAsync(int id);
        Task<ServiceResult> RegistrarEntradaAsync(int id, DateTime fechaHora);
        Task<ServiceResult> RegistrarSalidaAsync(int id);
        Task<IEnumerable<VisitaDTO>> GetByVisitanteIdAsync(int visitanteId);
        Task<IEnumerable<VisitaDTO>> GetByEstadoAsync(string estado);
    }
}
