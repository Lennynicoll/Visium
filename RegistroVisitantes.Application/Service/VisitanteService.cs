using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.Visitante;
using RegistroVisitantes.Domain.Entities;
using RegistroVisitantes.Infrastructure.Interfaces;

namespace RegistroVisitantes.Application.Service
{
    public class VisitanteService : BaseService<VisitanteDTO>, IVisitanteService
    {
        private readonly IVisitanteRepository _visitanteRepository;

        public VisitanteService(IVisitanteRepository visitanteRepository)
        {
            _visitanteRepository = visitanteRepository;
        }

        public async Task<ServiceResult> CreateWithValidationAsync(VisitanteDTO dto)
        {
            var errors = ValidateVisitante(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var visitante = new Visitante(dto.Nombre, dto.Apellido, dto.Correo, dto.Telefono);

            var created = await _visitanteRepository.CreateAsync(visitante);

            return ServiceResult.Ok(MapToDTO(created), "Visitante creado exitosamente");
        }

        public async Task<ServiceResult> UpdateWithValidationAsync(int id, VisitanteDTO dto)
        {
            var existing = await _visitanteRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Visitante con ID {id} no encontrado");

            var errors = ValidateVisitante(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var visitante = new Visitante(dto.Nombre, dto.Apellido, dto.Correo, dto.Telefono)
            {
                Id = id
            };

            var updated = await _visitanteRepository.UpdateAsync(id, visitante);

            return ServiceResult.Ok(MapToDTO(updated!), "Visitante actualizado exitosamente");
        }

        public override async Task<IEnumerable<VisitanteDTO>> GetAllAsync()
        {
            var visitantes = await _visitanteRepository.GetAllAsync();
            return visitantes.Select(MapToDTO);
        }

        public override async Task<VisitanteDTO?> GetByIdAsync(int id)
        {
            var visitante = await _visitanteRepository.GetByIdAsync(id);
            if (visitante == null) return null;

            return MapToDTO(visitante);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await _visitanteRepository.DeleteAsync(id);
        }

        private static VisitanteDTO MapToDTO(Visitante entity)
        {
            return new VisitanteDTO
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                Correo = entity.Correo,
                Telefono = entity.Telefono
            };
        }

        private List<string> ValidateVisitante(VisitanteDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errors.Add("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(dto.Apellido))
                errors.Add("El apellido es requerido");

            if (string.IsNullOrWhiteSpace(dto.Correo))
                errors.Add("El correo es requerido");
            else if (!dto.Correo.Contains("@") || !dto.Correo.Contains("."))
                errors.Add("El correo no tiene un formato válido");

            if (!string.IsNullOrWhiteSpace(dto.Telefono) && dto.Telefono.Length < 7)
                errors.Add("El teléfono debe tener al menos 7 caracteres");

            return errors;
        }
    }
}
