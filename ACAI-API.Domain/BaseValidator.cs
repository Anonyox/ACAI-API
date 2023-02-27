using FluentValidation;

namespace ACAI_API.Domain
{
    //Validação Entidade
    public abstract class BaseValidator<TId, TEntity> : AbstractValidator<TEntity>
		where TEntity : BaseEntity<TId>
	{
	}

	public abstract class BaseValidator<TEntity> : BaseValidator<Guid, TEntity>
		where TEntity : BaseEntity<Guid>
	{
	}
}
