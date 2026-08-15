using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.SeguridadEdificio;
using RegistroVisitantes.Domain.Entities;
using RegistroVisitantes.Infrastructure.Interfaces;

namespace RegistroVisitantes.Application.Service
{
    public class SeguridadEdificioService : BaseService<SeguridadEdificioDTO>, ISeguridadEdificioService
    {
        private readonly ISeguridadEdificioRepository _seguridadEdificioRepository;

        public SeguridadEdificioService(ISeguridadEdificioRepository seguridadEdificioRepository)
        {
            _seguridadEdificioRepository = seguridadEdificioRepository;
        }

        public async Task<ServiceResult> CreateWithValidationAsync(SeguridadEdificioDTO dto)
        {
            var errors = ValidateSeguridad(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var seguridad = new SeguridadEdificio(dto.Nombre, dto.Empresa, dto.Telefono, dto.Cobertura);

            var created = await _seguridadEdificioRepository.CreateAsync(seguridad);

            return ServiceResult.Ok(MapToDTO(created), "Registro de seguridad creado exitosamente");
        }

        public async Task<ServiceResult> UpdateWithValidationAsync(int id, SeguridadEdificioDTO dto)
        {
            var existing = await _seguridadEdificioRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Registro de seguridad con ID {id} no encontrado");

            var errors = ValidateSeguridad(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var seguridad = new SeguridadEdificio(dto.Nombre, dto.Empresa, dto.Telefono, dto.Cobertura)
            {
                Id = id
            };

            var updated = await _seguridadEdificioRepository.UpdateAsync(id, seguridad);

            return ServiceResult.Ok(MapToDTO(updated!), "Registro de seguridad actualizado exitosamente");
        }

        public override async Task<IEnumerable<SeguridadEdificioDTO>> GetAllAsync()
        {
            var registros = await _seguridadEdificioRepository.GetAllAsync();
            return registros.Select(MapToDTO);
        }

        public override async Task<SeguridadEdificioDTO?> GetByIdAsync(int id)
        {
            var registro = await _seguridadEdificioRepository.GetByIdAsync(id);
            if (registro == null) return null;

            return MapToDTO(registro);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await _seguridadEdificioRepository.DeleteAsync(id);
        }

        private static SeguridadEdificioDTO MapToDTO(SeguridadEdificio entity)
        {
            return new SeguridadEdificioDTO
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Empresa = entity.Empresa,
                Telefono = entity.Telefono,
                Cobertura = entity.Cobertura
            };
        }

        private List<string> ValidateSeguridad(SeguridadEdificioDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errors.Add("El nombre es requerido");
            else if (dto.Nombre.Length > 150)
                errors.Add("El nombre no puede exceder 150 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Empresa))
                errors.Add("La empresa es requerida");
            else if (dto.Empresa.Length > 150)
                errors.Add("La empresa no puede exceder 150 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Telefono))
                errors.Add("El teléfono es requerido");
            else if (dto.Telefono.Length < 7)
                errors.Add("El teléfono debe tener al menos 7 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Cobertura))
                errors.Add("La cobertura es requerida");
            else if (dto.Cobertura.Length > 200)
                errors.Add("La cobertura no puede exceder 200 caracteres");

            return errors;
        }
    }
}
