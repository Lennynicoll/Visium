using RegistroVisitantes.Domain.Core;

namespace RegistroVisitantes.Domain.Entities
{
    public class Visita : BaseEntity
    {
        public Visita()
            : base()
        {
        }

        public Visita(DateTime fechaHora, string motivo, string comentarios, int visitanteId, int anfitrionId)
            : this(fechaHora, motivo, comentarios, visitanteId, anfitrionId, "Pendiente", null, null)
        {
        }

        public Visita(DateTime fechaHora, string motivo, string comentarios, int visitanteId, int anfitrionId, string estado, DateTime? fechaEntrada, DateTime? fechaSalida)
            : base()
        {
            FechaHora = fechaHora;
            Motivo = motivo;
            Comentarios = comentarios;
            VisitanteId = visitanteId;
            AnfitrionId = anfitrionId;
            Estado = estado;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
        }

        public DateTime FechaHora { get; private set; }
        public string Motivo { get; private set; } = string.Empty;
        public string Comentarios { get; private set; } = string.Empty;
        public DateTime? FechaEntrada { get; private set; }
        public DateTime? FechaSalida { get; private set; }
        public string Estado { get; private set; } = "Pendiente";

        public int VisitanteId { get; private set; }
        public Visitante? Visitante { get; private set; }

        public int AnfitrionId { get; private set; }
        public Anfitrion? Anfitrion { get; private set; }

        public void RegistrarEntrada(DateTime fechaHora)
        {
            FechaEntrada = fechaHora;
            Estado = "En Curso";
        }

        public void RegistrarSalida(DateTime fechaHora)
        {
            FechaSalida = fechaHora;
            Estado = "Finalizada";
        }

        public override string ObtenerResumen()
        {
            return $"Visita del {FechaHora:dd/MM/yyyy HH:mm} - {Motivo} [{Estado}]";
        }
    }
}
