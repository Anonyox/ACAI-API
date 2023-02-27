using ACAI_API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACAI_API.Data.EF
{
	public class BaseRepositoryEntityFramework<TKey, TEntity> : IBaseRepository<TKey, TEntity>
		  where TEntity : BaseEntity<TKey>
	{
		protected DbContext Context { get; }

		protected DbSet<TEntity> DbSet => Context.Set<TEntity>();

		protected BaseRepositoryEntityFramework(DbContext context)
		{
			Context = context;
		}

		public Task<T> LoadEntityAsync<T>(object id)
		{
			throw new System.NotImplementedException();
		}

		public async Task<TEntity> LoadAsync(TKey id)
		{
			return await DbSet.FindAsync(id);
		}

		public virtual async Task<TEntity> GetAsync(TKey id)
		{
			return await DbSet.FindAsync(id);
		}

		public virtual async Task<IEnumerable<TEntity>> FindAsync()
		{
			return await DbSet.ToListAsync();
		}

		public virtual async Task AddAsync(TEntity entity)
		{
			await DbSet.AddAsync(entity);
		}

		public virtual async Task DeleteAsync(TEntity entity)
		{
			await Task.Factory.StartNew(() => DbSet.Remove(entity));
		}

		public async Task ApplyChanges()
		{
			await Context.SaveChangesAsync();
		}
	}

	public class BaseRepositoryEntityFramework<TEntity> : BaseRepositoryEntityFramework<Guid, TEntity>
		where TEntity : BaseEntity<Guid>
	{
		protected BaseRepositoryEntityFramework(DbContext context)
			: base(context)
		{
		}
	}
}
