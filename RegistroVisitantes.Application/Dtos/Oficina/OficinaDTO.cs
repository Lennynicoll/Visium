using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Application.Dtos.Oficina
{
    public class OficinaDTO : DtoBase
    {
        [Required(ErrorMessage = "El nombre de la oficina es requerido")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ubicación es requerida")]
        [StringLength(150, ErrorMessage = "La ubicación no puede exceder 150 caracteres")]
        public string Ubicacion { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "La extensión no puede exceder 20 caracteres")]
        public string Extension { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
