using RegistroVisitantes.Domain.Entities;
using RegistroVisitantes.Infrastructure.Core;
using RegistroVisitantes.Infrastructure.Context;
using RegistroVisitantes.Infrastructure.Interfaces;

namespace RegistroVisitantes.Infrastructure.Repositories
{
    public class DocumentoRepository : BaseRepository<Documento>, IDocumentoRepository
    {
        public DocumentoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
