using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.Oficina;
using RegistroVisitantes.Domain.Entities;
using RegistroVisitantes.Infrastructure.Interfaces;

namespace RegistroVisitantes.Application.Service
{
    public class OficinaService : BaseService<OficinaDTO>, IOficinaService
    {
        private readonly IOficinaRepository _oficinaRepository;

        public OficinaService(IOficinaRepository oficinaRepository)
        {
            _oficinaRepository = oficinaRepository;
        }

        public async Task<ServiceResult> CreateWithValidationAsync(OficinaDTO dto)
        {
            var errors = ValidateOficina(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var oficina = new Oficina(dto.Nombre, dto.Ubicacion, dto.Extension, dto.Descripcion);

            var created = await _oficinaRepository.CreateAsync(oficina);

            return ServiceResult.Ok(MapToDTO(created), "Oficina creada exitosamente");
        }

        public async Task<ServiceResult> UpdateWithValidationAsync(int id, OficinaDTO dto)
        {
            var existing = await _oficinaRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Oficina con ID {id} no encontrada");

            var errors = ValidateOficina(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var oficina = new Oficina(dto.Nombre, dto.Ubicacion, dto.Extension, dto.Descripcion)
            {
                Id = id
            };

            var updated = await _oficinaRepository.UpdateAsync(id, oficina);

            return ServiceResult.Ok(MapToDTO(updated!), "Oficina actualizada exitosamente");
        }

        public async Task<IEnumerable<OficinaDTO>> SearchByNameAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return await GetAllAsync();

            var oficinas = await _oficinaRepository.GetAllAsync();
            return oficinas
                .Where(o => o.Nombre.Contains(nombre))
                .Select(MapToDTO);
        }

        public override async Task<IEnumerable<OficinaDTO>> GetAllAsync()
        {
            var oficinas = await _oficinaRepository.GetAllAsync();
            return oficinas.Select(MapToDTO);
        }

        public override async Task<OficinaDTO?> GetByIdAsync(int id)
        {
            var oficina = await _oficinaRepository.GetByIdAsync(id);
            if (oficina == null) return null;

            return MapToDTO(oficina);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await _oficinaRepository.DeleteAsync(id);
        }

        private static OficinaDTO MapToDTO(Oficina entity)
        {
            return new OficinaDTO
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Ubicacion = entity.Ubicacion,
                Extension = entity.Extension,
                Descripcion = entity.Descripcion
            };
        }

        private List<string> ValidateOficina(OficinaDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errors.Add("El nombre de la oficina es requerido");
            else if (dto.Nombre.Length > 150)
                errors.Add("El nombre no puede exceder 150 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Ubicacion))
                errors.Add("La ubicación es requerida");
            else if (dto.Ubicacion.Length > 150)
                errors.Add("La ubicación no puede exceder 150 caracteres");

            if (!string.IsNullOrWhiteSpace(dto.Extension) && dto.Extension.Length > 20)
                errors.Add("La extensión no puede exceder 20 caracteres");

            if (!string.IsNullOrWhiteSpace(dto.Descripcion) && dto.Descripcion.Length > 500)
                errors.Add("La descripción no puede exceder 500 caracteres");

            return errors;
        }
    }
}
