using Microsoft.EntityFrameworkCore;
using RegistroVisitantes.Domain.Entities;

namespace RegistroVisitantes.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Visitante> Visitantes { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Anfitrion> Anfitriones { get; set; }
        public DbSet<MotivoVisita> MotivosVisita { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<SeguridadEdificio> SeguridadEdificios { get; set; }
    }
}
