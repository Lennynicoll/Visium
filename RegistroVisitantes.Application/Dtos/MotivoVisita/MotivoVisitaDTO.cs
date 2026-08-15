using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Application.Dtos.MotivoVisita
{
    public class MotivoVisitaDTO : DtoBase
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
