using ACAI_API.Domain.Util.Validation;

namespace ACAI_API.Domain
{
    public interface IBaseService<in TId, TEntity>
		 where TEntity : BaseEntity<TId>
	{
		//Adicionar Commands
		Task<ValidateableResponse<TEntity>> AddAsync<TCommand>(TCommand t)
			where TCommand : BaseCommand;

		//Alterar commands
		Task<ValidateableResponse<TEntity>> UpdateAsync<TCommand>(TId key, TCommand t)
			where TCommand : BaseCommand;

		//Deletar commands
		Task DeleteAsync(TId key);
	}

	//Herda da classe acima e Chave primaria é long
	public interface IBaseService<TEntity> : IBaseService<Guid, TEntity>
		where TEntity : BaseEntity<Guid>
	{
	}
}
