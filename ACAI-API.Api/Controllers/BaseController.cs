using ACAI_API.Api.Util.Handlers;
using ACAI_API.Domain;
using ACAI_API.Api.Util.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace ACAI_API.Api.Controllers
{
	[ApiController]
	public abstract class BaseController : ControllerBase
	{
		protected new IActionResult UnprocessableEntity(object errors)
		{
			return StatusCode(StatusCodes.Status422UnprocessableEntity, new Util.Handlers.ProblemDetails()
			{
				Title = ProblemConstants.UnprocessableEntityTitle,
				Type = ProblemConstants.UnprocessableEntityType,
				Instance = Request.Path,
				Status = StatusCodes.Status422UnprocessableEntity,
				Detail = errors
			});
		}
	}

	[Authorize]
	public abstract class BaseController<TId, TEntity, TPost, TPut> : BaseController
		where TEntity : BaseEntity<TId>
		where TPost : BaseCommand
		where TPut : BaseCommand
	{
		private readonly IBaseRepository<TId, TEntity> _repository;
		private readonly IBaseService<TId, TEntity> _service;

		protected BaseController(IBaseRepository<TId, TEntity> repository,
			IBaseService<TId, TEntity> service)
		{
			_repository = repository;
			_service = service;
		}

		[HttpGet("{id}")]
		public virtual async Task<IActionResult> Get(TId id)
		{
			var entity = await _repository.GetAsync(id);

			if (entity == null)
				return NotFound();

			return Ok(entity);
		}

		[HttpGet]
		public virtual async Task<IActionResult> Get()
		{
			// var userIdentity = User?.Identity;

			var entities = await _repository.FindAsync();

			return Ok(entities);
		}

		[HttpPost]
		[AsyncTransaction]
		public virtual async Task<IActionResult> Post([FromBody] TPost command)
		{
			var result = await _service.AddAsync(command);
			if (!result.Success)
			{
				return UnprocessableEntity(result.Errors);
			}

			return CreatedAtAction(nameof(Get), new { id = result.Model.Id });
		}

		[HttpPut("{id}")]
		[AsyncTransaction]
		public virtual async Task<IActionResult> Put(TId id, [FromBody] TPut command)
		{
			var result = await _service.UpdateAsync(id, command);

			if (result == null)
			{
				return NotFound();
			}

			if (!result.Success)
			{
				return UnprocessableEntity(result.Errors);
			}

			return Ok();
		}

		[HttpDelete("{id}")]
		[AsyncTransaction]
		public virtual async Task<IActionResult> Delete(TId id)
		{
			await _service.DeleteAsync(id);

			return NoContent();
		}
	}

	public abstract class BaseController<TEntity, TPost, TPut> : BaseController<Guid, TEntity, TPost, TPut>
		where TEntity : BaseEntity<Guid>
		where TPost : BaseCommand
		where TPut : BaseCommand
	{
		protected BaseController(IBaseRepository<Guid, TEntity> repository,
			IBaseService<Guid, TEntity> service)
			: base(repository, service)
		{
		}
	}

	public abstract class BaseController<TEntity, TPost> : BaseController<Guid, TEntity, TPost, TPost>
		where TEntity : BaseEntity<Guid>
		where TPost : BaseCommand
	{
		protected BaseController(IBaseRepository<Guid, TEntity> repository,
			IBaseService<Guid, TEntity> service)
			: base(repository, service)
		{
		}
	}
}