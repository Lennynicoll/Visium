using RegistroVisitantes.Application.Contract;
using RegistroVisitantes.Application.Core;
using RegistroVisitantes.Application.Dtos.Documento;
using RegistroVisitantes.Domain.Entities;
using RegistroVisitantes.Infrastructure.Interfaces;

namespace RegistroVisitantes.Application.Service
{
    public class DocumentoService : BaseService<DocumentoDTO>, IDocumentoService
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IVisitanteRepository _visitanteRepository;

        public DocumentoService(IDocumentoRepository documentoRepository, IVisitanteRepository visitanteRepository)
        {
            _documentoRepository = documentoRepository;
            _visitanteRepository = visitanteRepository;
        }

        public async Task<ServiceResult> CreateWithValidationAsync(DocumentoDTO dto)
        {
            var errors = await ValidateDocumento(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var documento = new Documento(dto.Tipo, dto.Numero, dto.FechaExpedicion, dto.FechaVencimiento, dto.VisitanteId);

            var created = await _documentoRepository.CreateAsync(documento);

            return ServiceResult.Ok(MapToDTO(created), "Documento registrado exitosamente");
        }

        public async Task<ServiceResult> UpdateWithValidationAsync(int id, DocumentoDTO dto)
        {
            var existing = await _documentoRepository.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult.Fail($"Documento con ID {id} no encontrado");

            var errors = await ValidateDocumento(dto);
            if (errors.Count > 0)
                return ServiceResult.Fail(errors);

            var documento = new Documento(dto.Tipo, dto.Numero, dto.FechaExpedicion, dto.FechaVencimiento, dto.VisitanteId)
            {
                Id = id
            };

            var updated = await _documentoRepository.UpdateAsync(id, documento);

            return ServiceResult.Ok(MapToDTO(updated!), "Documento actualizado exitosamente");
        }

        public async Task<IEnumerable<DocumentoDTO>> GetByVisitanteIdAsync(int visitanteId)
        {
            var documentos = await _documentoRepository.GetAllAsync();
            return documentos
                .Where(d => d.VisitanteId == visitanteId)
                .Select(MapToDTO);
        }

        public override async Task<IEnumerable<DocumentoDTO>> GetAllAsync()
        {
            var documentos = await _documentoRepository.GetAllAsync();
            return documentos.Select(MapToDTO);
        }

        public override async Task<DocumentoDTO?> GetByIdAsync(int id)
        {
            var documento = await _documentoRepository.GetByIdAsync(id);
            if (documento == null) return null;

            return MapToDTO(documento);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await _documentoRepository.DeleteAsync(id);
        }

        private static DocumentoDTO MapToDTO(Documento entity)
        {
            return new DocumentoDTO
            {
                Id = entity.Id,
                Tipo = entity.Tipo,
                Numero = entity.Numero,
                FechaExpedicion = entity.FechaExpedicion,
                FechaVencimiento = entity.FechaVencimiento,
                VisitanteId = entity.VisitanteId
            };
        }

        private async Task<List<string>> ValidateDocumento(DocumentoDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Tipo))
                errors.Add("El tipo de documento es requerido");
            else if (dto.Tipo.Length > 50)
                errors.Add("El tipo de documento no puede exceder 50 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Numero))
                errors.Add("El número de documento es requerido");
            else if (dto.Numero.Length > 50)
                errors.Add("El número de documento no puede exceder 50 caracteres");

            if (dto.FechaExpedicion == default)
                errors.Add("La fecha de expedición es requerida");

            if (dto.FechaVencimiento == default)
                errors.Add("La fecha de vencimiento es requerida");
            else if (dto.FechaVencimiento < dto.FechaExpedicion)
                errors.Add("La fecha de vencimiento no puede ser anterior a la expedición");

            if (dto.VisitanteId <= 0)
                errors.Add("El ID del visitante es requerido");
            else
            {
                var visitanteExists = await _visitanteRepository.GetByIdAsync(dto.VisitanteId);
                if (visitanteExists == null)
                    errors.Add($"El visitante con ID {dto.VisitanteId} no existe");
            }

            return errors;
        }
    }
}
