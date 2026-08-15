using RegistroVisitantes.Domain.Core;

namespace RegistroVisitantes.Domain.Entities
{
    public class Oficina : BaseEntity
    {
        public Oficina()
            : base()
        {
        }

        public Oficina(string nombre, string ubicacion, string extension, string descripcion)
            : base()
        {
            Nombre = nombre;
            Ubicacion = ubicacion;
            Extension = extension;
            Descripcion = descripcion;
        }

        public string Nombre { get; private set; } = string.Empty;
        public string Ubicacion { get; private set; } = string.Empty;
        public string Extension { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;

        public override string ObtenerResumen()
        {
            return $"Oficina: {Nombre} - {Ubicacion} (ext. {Extension})";
        }
    }
}
