using ACAI_API.Domain.Util.Validation;
using AutoMapper;
using FluentValidation;

namespace ACAI_API.Domain
{
    public abstract class BaseService<TId, TEntity> : IBaseService<TId, TEntity>
		 where TEntity : BaseEntity<TId>
	{
		protected IMapper Mapper { get; private set; }
		protected IBaseRepository<TId, TEntity> Repository { get; private set; }
		protected IValidator<TEntity> Validator { get; private set; }

		protected BaseService(IMapper mapper, IValidator<TEntity> validator, IBaseRepository<TId, TEntity> repository)
		{
			Mapper = mapper;
			Validator = validator;
			Repository = repository;
		}

		//Adicionar Entidade
		public virtual async Task<ValidateableResponse<TEntity>> AddAsync<TCommand>(TCommand t)
			where TCommand : BaseCommand
		{
			var entity = Mapper.Map<TEntity>(t);

			var validationResult = await Validator.ValidateAsync(entity);
			if (!validationResult.IsValid)
				return new ValidateableResponse<TEntity>(validationResult);

			await Repository.AddAsync(entity);
			await Repository.ApplyChanges();

			return new ValidateableResponse<TEntity>(entity);
		}

		//Alterar Entitade
		public virtual async Task<ValidateableResponse<TEntity>> UpdateAsync<TCommand>(TId id, TCommand t)
			where TCommand : BaseCommand
		{
			var entity = await Repository.GetAsync(id);
			if (entity == null)
				return null;

			Mapper.Map(t, entity);

			var validationResult = await Validator.ValidateAsync(entity);
			if (!validationResult.IsValid)
				return new ValidateableResponse<TEntity>(validationResult);

			await Repository.ApplyChanges();

			return new ValidateableResponse<TEntity>(entity);
		}

		//Deletar Entidade
		public virtual async Task DeleteAsync(TId id)
		{
			var entity = await Repository.GetAsync(id);
			if (entity == null)
				return;

			await Repository.DeleteAsync(entity);
			await Repository.ApplyChanges();
		}


		protected void SaveCollection<TChildEntity, TChidlCommand>(IEnumerable<TChildEntity> children, IEnumerable<TChidlCommand> commands, Action<TChildEntity> add, Action<TChildEntity> remove)
			where TChildEntity : BaseEntity
			where TChidlCommand : BaseCommand
		{
			var current = commands.Select(c => c.Id);
			var childrenToDelete = children.Where(x => !current.Contains(x.Id)).ToList();

			childrenToDelete.ForEach(remove);

			foreach (var command in commands)
			{
				if (command.Id == default)
				{
					var newInterval = Mapper.Map<TChildEntity>(command);

					add(newInterval);
				}
				else
				{
					var currentInterval = children.First(x => x.Id == command.Id);

					Mapper.Map(command, currentInterval);
				}
			}
		}

		protected async Task SaveCollection<TChildKey, TChildEntity>(IEnumerable<TChildKey> commandKeys, IEnumerable<TChildEntity> collection, Action<TChildEntity> add, Action<TChildEntity> remove)
			where TChildEntity : BaseModel<TChildKey>
		{
			var shiftsToDelete = collection.Where(x => !commandKeys.Contains(x.Id)).ToList();

			shiftsToDelete.ForEach(remove);

			var collectionIds = collection.Select(x => x.Id);

			foreach (var id in commandKeys.Where(x => !collectionIds.Contains(x)))
			{
				var reference = await Repository.LoadEntityAsync<TChildEntity>(id);

				add(reference);
			}
		}
	}

	public abstract class BaseService<TEntity> : BaseService<Guid, TEntity>, IBaseService<TEntity>
		where TEntity : BaseEntity<Guid>
	{
		protected BaseService(IMapper mapper, IValidator<TEntity> validator, IBaseRepository<Guid, TEntity> repository)
			: base(mapper, validator, repository)
		{
		}
	}
}
