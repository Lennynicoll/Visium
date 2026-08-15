using RegistroVisitantes.Domain.Core;

namespace RegistroVisitantes.Domain.Entities
{
    public class Departamento : BaseEntity
    {
        public Departamento()
            : base()
        {
        }

        public Departamento(string nombre, string descripcion, string ubicacion)
            : base()
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Ubicacion = ubicacion;
        }

        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public string Ubicacion { get; private set; } = string.Empty;

        public List<Anfitrion> Anfitriones { get; private set; } = new List<Anfitrion>();

        public override string ObtenerResumen()
        {
            return $"Departamento: {Nombre} - {Ubicacion}";
        }
    }
}
