namespace RegistroVisitantes.Domain.Core
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = 0;
        }

        protected BaseEntity(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public abstract string ObtenerResumen();
    }
}
