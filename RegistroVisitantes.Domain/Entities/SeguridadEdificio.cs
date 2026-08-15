using RegistroVisitantes.Domain.Core;

namespace RegistroVisitantes.Domain.Entities
{
    public class SeguridadEdificio : BaseEntity
    {
        public SeguridadEdificio()
            : base()
        {
        }

        public SeguridadEdificio(string nombre, string empresa, string telefono, string cobertura)
            : base()
        {
            Nombre = nombre;
            Empresa = empresa;
            Telefono = telefono;
            Cobertura = cobertura;
        }

        public string Nombre { get; private set; } = string.Empty;
        public string Empresa { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;
        public string Cobertura { get; private set; } = string.Empty;

        public override string ObtenerResumen()
        {
            return $"Seguridad: {Nombre} ({Empresa}) - {Cobertura}";
        }
    }
}
