using RegistroVisitantes.Domain.Core;

namespace RegistroVisitantes.Domain.Entities
{
    public class Anfitrion : BaseEntity
    {
        public Anfitrion()
            : base()
        {
        }

        public Anfitrion(string nombre, string apellido, string cedula, string telefono, string correo, string horarioAtencion, int departamentoId, int motivoVisitaId)
            : base()
        {
            Nombre = nombre;
            Apellido = apellido;
            Cedula = cedula;
            Telefono = telefono;
            Correo = correo;
            HorarioAtencion = horarioAtencion;
            DepartamentoId = departamentoId;
            MotivoVisitaId = motivoVisitaId;
        }

        public string Nombre { get; private set; } = string.Empty;
        public string Apellido { get; private set; } = string.Empty;
        public string Cedula { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;
        public string Correo { get; private set; } = string.Empty;
        public string HorarioAtencion { get; private set; } = string.Empty;

        public int MotivoVisitaId { get; private set; }
        public MotivoVisita? MotivoVisita { get; private set; }

        public int DepartamentoId { get; private set; }
        public Departamento? Departamento { get; private set; }

        public List<Visita> Visitas { get; private set; } = new List<Visita>();

        public override string ObtenerResumen()
        {
            return $"Anfitrión: {Nombre} {Apellido} - Depto. {DepartamentoId}";
        }
    }
}
