using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.Visita;
using RegistroVisitantes.Domain.Entities;
using RegistroVisitantes.Infrastructure.Interfaces;

namespace RegistroVisitantes.Application.Service
{
    public class VisitaService : BaseService<VisitaDTO>, IVisitaService
    {
        private readonly IVisitaRepository _visitaRepository;
        private readonly IVisitanteRepository _visitanteRepository;
        private readonly IAnfitrionRepository _anfitrionRepository;

        public VisitaService(IVisitaRepository visitaRepository, IVisitanteRepository visitanteRepository, IAnfitrionRepository anfitrionRepository)
        {
            _visitaRepository = visitaRepository;
            _visitanteRepository = visitanteRepository;
            _anfitrionRepository = anfitrionRepository;
        }

        public async Task<ServiceResult> CreateWithValidationAsync(VisitaDTO dto)
        {
            var errors = await ValidateVisita(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var estado = string.IsNullOrWhiteSpace(dto.Estado) ? "Pendiente" : dto.Estado;
            var visita = new Visita(dto.FechaHora, dto.Motivo, dto.Comentarios, dto.VisitanteId, dto.AnfitrionId, estado, dto.FechaEntrada, dto.FechaSalida);

            var created = await _visitaRepository.CreateAsync(visita);

            return ServiceResult.Ok(MapToDTO(created), "Visita creada exitosamente");
        }

        public async Task<ServiceResult> UpdateWithValidationAsync(int id, VisitaDTO dto)
        {
            var existing = await _visitaRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Visita con ID {id} no encontrada");

            var errors = await ValidateVisita(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var visita = new Visita(dto.FechaHora, dto.Motivo, dto.Comentarios, dto.VisitanteId, dto.AnfitrionId, dto.Estado, dto.FechaEntrada, dto.FechaSalida)
            {
                Id = id
            };

            var updated = await _visitaRepository.UpdateAsync(id, visita);

            return ServiceResult.Ok(MapToDTO(updated!), "Visita actualizada exitosamente");
        }

        public async Task<ServiceResult> RegistrarEntradaAsync(int id)
        {
            return await RegistrarEntradaAsync(id, DateTime.Now);
        }

        public async Task<ServiceResult> RegistrarEntradaAsync(int id, DateTime fechaHora)
        {
            var existing = await _visitaRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Visita con ID {id} no encontrada");

            existing.RegistrarEntrada(fechaHora);

            await _visitaRepository.UpdateAsync(id, existing);

            return ServiceResult.Ok(MapToDTO(existing), "Entrada registrada exitosamente");
        }

        public async Task<ServiceResult> RegistrarSalidaAsync(int id)
        {
            var existing = await _visitaRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Visita con ID {id} no encontrada");

            existing.RegistrarSalida(DateTime.Now);

            await _visitaRepository.UpdateAsync(id, existing);

            return ServiceResult.Ok(MapToDTO(existing), "Salida registrada exitosamente");
        }

        public async Task<IEnumerable<VisitaDTO>> GetByVisitanteIdAsync(int visitanteId)
        {
            var visitas = await _visitaRepository.GetAllAsync();
            return visitas
                .Where(v => v.VisitanteId == visitanteId)
                .Select(MapToDTO);
        }

        public async Task<IEnumerable<VisitaDTO>> GetByEstadoAsync(string estado)
        {
            var visitas = await _visitaRepository.GetAllAsync();
            return visitas
                .Where(v => v.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDTO);
        }

        public override async Task<IEnumerable<VisitaDTO>> GetAllAsync()
        {
            var visitas = await _visitaRepository.GetAllAsync();
            return visitas.Select(MapToDTO);
        }

        public override async Task<VisitaDTO?> GetByIdAsync(int id)
        {
            var visita = await _visitaRepository.GetByIdAsync(id);
            if (visita == null) return null;

            return MapToDTO(visita);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await _visitaRepository.DeleteAsync(id);
        }

        private static VisitaDTO MapToDTO(Visita entity)
        {
            return new VisitaDTO
            {
                Id = entity.Id,
                FechaHora = entity.FechaHora,
                Motivo = entity.Motivo,
                Comentarios = entity.Comentarios,
                FechaEntrada = entity.FechaEntrada,
                FechaSalida = entity.FechaSalida,
                Estado = entity.Estado,
                VisitanteId = entity.VisitanteId,
                AnfitrionId = entity.AnfitrionId
            };
        }

        private async Task<List<string>> ValidateVisita(VisitaDTO dto)
        {
            var errors = new List<string>();

            if (dto.FechaHora == default)
                errors.Add("La fecha y hora son requeridas");

            if (string.IsNullOrWhiteSpace(dto.Motivo))
                errors.Add("El motivo es requerido");
            else if (dto.Motivo.Length < 3)
                errors.Add("El motivo debe tener al menos 3 caracteres");

            if (dto.VisitanteId <= 0)
                errors.Add("El ID del visitante es requerido");
            else
            {
                var visitanteExists = await _visitanteRepository.GetByIdAsync(dto.VisitanteId);
                if (visitanteExists == null)
                    errors.Add($"El visitante con ID {dto.VisitanteId} no existe");
            }

            if (dto.AnfitrionId > 0)
            {
                var anfitrionExists = await _anfitrionRepository.GetByIdAsync(dto.AnfitrionId);
                if (anfitrionExists == null)
                    errors.Add($"El anfitrión con ID {dto.AnfitrionId} no existe");
            }

            return errors;
        }
    }
}
