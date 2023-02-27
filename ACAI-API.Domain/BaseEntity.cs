namespace ACAI_API.Domain
{
    public abstract class BaseEntity<TId>
    {
        public virtual TId ?Id { get; set; }

        public virtual DateTime Insercion_Date { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<Guid>
    {

    }
}
