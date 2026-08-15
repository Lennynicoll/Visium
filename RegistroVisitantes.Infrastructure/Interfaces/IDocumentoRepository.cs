using RegistroVisitantes.Domain.Entities;

namespace RegistroVisitantes.Infrastructure.Interfaces
{
    public interface IDocumentoRepository
    {
        Task<IEnumerable<Documento>> GetAllAsync();
        Task<Documento?> GetByIdAsync(int id);
        Task<Documento> CreateAsync(Documento documento);
        Task<Documento?> UpdateAsync(int id, Documento documento);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
