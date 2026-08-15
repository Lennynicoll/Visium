using RegistroVisitantes.Domain.Core;

namespace RegistroVisitantes.Domain.Entities
{
    public class Documento : BaseEntity
    {
        public Documento()
            : base()
        {
        }

        public Documento(string tipo, string numero, DateTime fechaExpedicion, DateTime fechaVencimiento, int visitanteId)
            : base()
        {
            Tipo = tipo;
            Numero = numero;
            FechaExpedicion = fechaExpedicion;
            FechaVencimiento = fechaVencimiento;
            VisitanteId = visitanteId;
        }

        public string Tipo { get; private set; } = string.Empty;
        public string Numero { get; private set; } = string.Empty;
        public DateTime FechaExpedicion { get; private set; }
        public DateTime FechaVencimiento { get; private set; }

        public int VisitanteId { get; private set; }
        public Visitante? Visitante { get; private set; }

        public bool EsVigente()
        {
            return FechaVencimiento >= DateTime.Today;
        }

        public override string ObtenerResumen()
        {
            return $"{Tipo} {Numero} (vence: {FechaVencimiento:dd/MM/yyyy})";
        }
    }
}
