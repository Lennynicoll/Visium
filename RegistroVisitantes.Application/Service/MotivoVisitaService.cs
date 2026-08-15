using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.MotivoVisita;
using RegistroVisitantes.Domain.Entities;
using RegistroVisitantes.Infrastructure.Interfaces;

namespace RegistroVisitantes.Application.Service
{
    public class MotivoVisitaService : BaseService<MotivoVisitaDTO>, IMotivoVisitaService
    {
        private readonly IMotivoVisitaRepository _motivoVisitaRepository;

        public MotivoVisitaService(IMotivoVisitaRepository motivoVisitaRepository)
        {
            _motivoVisitaRepository = motivoVisitaRepository;
        }

        public async Task<ServiceResult> CreateWithValidationAsync(MotivoVisitaDTO dto)
        {
            var errors = ValidateMotivo(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var motivo = new MotivoVisita(dto.Nombre, dto.Descripcion);

            var created = await _motivoVisitaRepository.CreateAsync(motivo);

            return ServiceResult.Ok(MapToDTO(created), "Motivo de visita creado exitosamente");
        }

        public async Task<ServiceResult> UpdateWithValidationAsync(int id, MotivoVisitaDTO dto)
        {
            var existing = await _motivoVisitaRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Motivo de visita con ID {id} no encontrado");

            var errors = ValidateMotivo(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var motivo = new MotivoVisita(dto.Nombre, dto.Descripcion)
            {
                Id = id
            };

            var updated = await _motivoVisitaRepository.UpdateAsync(id, motivo);

            return ServiceResult.Ok(MapToDTO(updated!), "Motivo de visita actualizado exitosamente");
        }

        public override async Task<IEnumerable<MotivoVisitaDTO>> GetAllAsync()
        {
            var motivos = await _motivoVisitaRepository.GetAllAsync();
            return motivos.Select(MapToDTO);
        }

        public override async Task<MotivoVisitaDTO?> GetByIdAsync(int id)
        {
            var motivo = await _motivoVisitaRepository.GetByIdAsync(id);
            if (motivo == null) return null;

            return MapToDTO(motivo);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await _motivoVisitaRepository.DeleteAsync(id);
        }

        private static MotivoVisitaDTO MapToDTO(MotivoVisita entity)
        {
            return new MotivoVisitaDTO
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion
            };
        }

        private List<string> ValidateMotivo(MotivoVisitaDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errors.Add("El nombre es requerido");
            else if (dto.Nombre.Length > 100)
                errors.Add("El nombre no puede exceder 100 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                errors.Add("La descripción es requerida");
            else if (dto.Descripcion.Length > 500)
                errors.Add("La descripción no puede exceder 500 caracteres");

            return errors;
        }
    }
}
