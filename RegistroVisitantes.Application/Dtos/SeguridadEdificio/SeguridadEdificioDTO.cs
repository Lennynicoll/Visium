using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Application.Dtos.SeguridadEdificio
{
    public class SeguridadEdificioDTO : DtoBase
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La empresa es requerida")]
        [StringLength(150, ErrorMessage = "La empresa no puede exceder 150 caracteres")]
        public string Empresa { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener al menos 7 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cobertura es requerida")]
        [StringLength(200, ErrorMessage = "La cobertura no puede exceder 200 caracteres")]
        public string Cobertura { get; set; } = string.Empty;
    }
}
