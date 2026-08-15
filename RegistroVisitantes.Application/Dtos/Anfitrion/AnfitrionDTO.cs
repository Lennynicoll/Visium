using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Application.Dtos.Anfitrion
{
    public class AnfitrionDTO : DtoBase
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es requerida")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "La cédula debe tener entre 10 y 13 caracteres")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener al menos 7 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
        public string Correo { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El horario de atención no puede exceder 100 caracteres")]
        public string HorarioAtencion { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "El ID del motivo de visita es requerido")]
        public int MotivoVisitaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El ID de departamento es requerido")]
        public int DepartamentoId { get; set; }
    }
}
