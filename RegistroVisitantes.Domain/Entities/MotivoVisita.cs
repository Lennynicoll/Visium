using RegistroVisitantes.Domain.Core;

namespace RegistroVisitantes.Domain.Entities
{
    public class MotivoVisita : BaseEntity
    {
        public MotivoVisita()
            : base()
        {
        }

        public MotivoVisita(string nombre, string descripcion)
            : base()
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;

        public List<Anfitrion> Anfitriones { get; private set; } = new List<Anfitrion>();

        public override string ObtenerResumen()
        {
            return $"Motivo de visita: {Nombre}";
        }
    }
}
