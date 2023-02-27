namespace ACAI_API.Domain
{
    public abstract class BaseDto : BaseModel<Guid>
    {
        public DateTime Insercion_Date { get; set; }
    }
}
