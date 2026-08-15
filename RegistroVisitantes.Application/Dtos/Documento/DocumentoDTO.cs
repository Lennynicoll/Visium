using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Application.Dtos.Documento
{
    public class DocumentoDTO : DtoBase
    {
        [Required(ErrorMessage = "El tipo de documento es requerido")]
        [StringLength(50, ErrorMessage = "El tipo de documento no puede exceder 50 caracteres")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es requerido")]
        [StringLength(50, ErrorMessage = "El número de documento no puede exceder 50 caracteres")]
        public string Numero { get; set; } = string.Empty;

        public DateTime FechaExpedicion { get; set; }
        public DateTime FechaVencimiento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El ID del visitante es requerido")]
        public int VisitanteId { get; set; }
    }
}
