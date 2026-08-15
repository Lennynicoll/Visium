using System.ComponentModel.DataAnnotations;

namespace RegistroVisitantes.Application.Dtos.Visita
{
    public class VisitaDTO : DtoBase
    {
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El motivo es requerido")]
        [MinLength(3, ErrorMessage = "El motivo debe tener al menos 3 caracteres")]
        public string Motivo { get; set; } = string.Empty;

        public string Comentarios { get; set; } = string.Empty;
        public DateTime? FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string Estado { get; set; } = "Pendiente";

        [Range(1, int.MaxValue, ErrorMessage = "El ID del visitante es requerido")]
        public int VisitanteId { get; set; }

        public int AnfitrionId { get; set; }
    }
}
