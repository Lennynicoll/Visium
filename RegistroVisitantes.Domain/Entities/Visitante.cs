using RegistroVisitantes.Domain.Core;

namespace RegistroVisitantes.Domain.Entities
{
    public class Visitante : BaseEntity
    {
        public Visitante()
            : base()
        {
        }

        public Visitante(string nombre, string apellido, string correo, string telefono)
            : base()
        {
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            Telefono = telefono;
        }

        public string Nombre { get; private set; } = string.Empty;
        public string Apellido { get; private set; } = string.Empty;
        public string Correo { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;

        public List<Documento> Documentos { get; private set; } = new List<Documento>();
        public List<Visita> Visitas { get; private set; } = new List<Visita>();

        public string NombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        public override string ObtenerResumen()
        {
            return $"Visitante: {NombreCompleto()}";
        }
    }
}
