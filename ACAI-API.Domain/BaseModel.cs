namespace ACAI_API.Domain
{
    public abstract class BaseModel<TId>
    {
        public virtual TId ?Id { get; set; }
    }
}
