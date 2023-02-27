namespace ACAI_API.Domain
{
    public interface IBaseRepository<in TId, TEntity>
		 where TEntity : BaseEntity<TId>
	{
		//Carregamento da entidade objeto
		Task<T> LoadEntityAsync<T>(object id);

		//Carregamento da entidade 
		Task<TEntity> LoadAsync(TId id);

		//Obter Entidade
		Task<TEntity> GetAsync(TId id);

		//Obter Entidade pela Chave Primaria
		Task<IEnumerable<TEntity>> FindAsync();

		//Adicionar registros da Entidade
		Task AddAsync(TEntity entity);

		//Remover registros da Entidade
		Task DeleteAsync(TEntity entity);

		//Salvar alterações
		Task ApplyChanges();
	}

	//Herda da classe acima e Mudando chave primaria para long
	public interface IBaseRepository<TEntity> : IBaseRepository<Guid, TEntity>
		where TEntity : BaseEntity<Guid>
	{
	}
}
