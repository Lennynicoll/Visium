using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.Anfitrion;
using RegistroVisitantes.Domain.Entities;
using RegistroVisitantes.Infrastructure.Interfaces;

namespace RegistroVisitantes.Application.Service
{
    public class AnfitrionService : BaseService<AnfitrionDTO>, IAnfitrionService
    {
        private readonly IAnfitrionRepository _anfitrionRepository;
        private readonly IMotivoVisitaRepository _motivoVisitaRepository;
        private readonly IDepartamentoRepository _departamentoRepository;

        public AnfitrionService(IAnfitrionRepository anfitrionRepository, IMotivoVisitaRepository motivoVisitaRepository, IDepartamentoRepository departamentoRepository)
        {
            _anfitrionRepository = anfitrionRepository;
            _motivoVisitaRepository = motivoVisitaRepository;
            _departamentoRepository = departamentoRepository;
        }

        public async Task<ServiceResult> CreateWithValidationAsync(AnfitrionDTO dto)
        {
            var errors = await ValidateAnfitrion(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var anfitrion = new Anfitrion(dto.Nombre, dto.Apellido, dto.Cedula, dto.Telefono, dto.Correo, dto.HorarioAtencion, dto.DepartamentoId, dto.MotivoVisitaId);

            var created = await _anfitrionRepository.CreateAsync(anfitrion);

            return ServiceResult.Ok(MapToDTO(created), "Anfitrión creado exitosamente");
        }

        public async Task<ServiceResult> UpdateWithValidationAsync(int id, AnfitrionDTO dto)
        {
            var existing = await _anfitrionRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Anfitrión con ID {id} no encontrado");

            var errors = await ValidateAnfitrion(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var anfitrion = new Anfitrion(dto.Nombre, dto.Apellido, dto.Cedula, dto.Telefono, dto.Correo, dto.HorarioAtencion, dto.DepartamentoId, dto.MotivoVisitaId)
            {
                Id = id
            };

            var updated = await _anfitrionRepository.UpdateAsync(id, anfitrion);

            return ServiceResult.Ok(MapToDTO(updated!), "Anfitrión actualizado exitosamente");
        }

        public async Task<IEnumerable<AnfitrionDTO>> GetByMotivoVisitaIdAsync(int motivoVisitaId)
        {
            var anfitriones = await _anfitrionRepository.GetAllAsync();
            return anfitriones
                .Where(a => a.MotivoVisitaId == motivoVisitaId)
                .Select(MapToDTO);
        }

        public override async Task<IEnumerable<AnfitrionDTO>> GetAllAsync()
        {
            var anfitriones = await _anfitrionRepository.GetAllAsync();
            return anfitriones.Select(MapToDTO);
        }

        public override async Task<AnfitrionDTO?> GetByIdAsync(int id)
        {
            var anfitrion = await _anfitrionRepository.GetByIdAsync(id);
            if (anfitrion == null) return null;

            return MapToDTO(anfitrion);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await _anfitrionRepository.DeleteAsync(id);
        }

        private static AnfitrionDTO MapToDTO(Anfitrion entity)
        {
            return new AnfitrionDTO
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                Cedula = entity.Cedula,
                Telefono = entity.Telefono,
                Correo = entity.Correo,
                HorarioAtencion = entity.HorarioAtencion,
                MotivoVisitaId = entity.MotivoVisitaId,
                DepartamentoId = entity.DepartamentoId
            };
        }

        private async Task<List<string>> ValidateAnfitrion(AnfitrionDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errors.Add("El nombre es requerido");
            else if (dto.Nombre.Length > 100)
                errors.Add("El nombre no puede exceder 100 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Apellido))
                errors.Add("El apellido es requerido");
            else if (dto.Apellido.Length > 100)
                errors.Add("El apellido no puede exceder 100 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Cedula))
                errors.Add("La cédula es requerida");
            else if (dto.Cedula.Length < 10 || dto.Cedula.Length > 13)
                errors.Add("La cédula debe tener entre 10 y 13 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Telefono))
                errors.Add("El teléfono es requerido");
            else if (dto.Telefono.Length < 7)
                errors.Add("El teléfono debe tener al menos 7 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Correo))
                errors.Add("El correo es requerido");
            else if (!dto.Correo.Contains("@") || !dto.Correo.Contains("."))
                errors.Add("El correo no tiene un formato válido");

            if (!string.IsNullOrWhiteSpace(dto.HorarioAtencion) && dto.HorarioAtencion.Length > 100)
                errors.Add("El horario de atención no puede exceder 100 caracteres");

            if (dto.MotivoVisitaId <= 0)
                errors.Add("El ID del motivo de visita es requerido");
            else
            {
                var motivoExists = await _motivoVisitaRepository.GetByIdAsync(dto.MotivoVisitaId);
                if (motivoExists == null)
                    errors.Add($"El motivo de visita con ID {dto.MotivoVisitaId} no existe");
            }

            if (dto.DepartamentoId <= 0)
                errors.Add("El ID de departamento es requerido");
            else
            {
                var departamentoExists = await _departamentoRepository.GetByIdAsync(dto.DepartamentoId);
                if (departamentoExists == null)
                    errors.Add($"El departamento con ID {dto.DepartamentoId} no existe");
            }

            return errors;
        }
    }
}
