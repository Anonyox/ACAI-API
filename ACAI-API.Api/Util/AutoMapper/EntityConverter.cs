using AutoMapper;
using ACAI_API.Data.EF;
using ACAI_API.Domain;

namespace ACAI_API.Api.Util.AutoMapper
{
	public class EntityConverter<TEntity> : ITypeConverter<long, TEntity>, ITypeConverter<long?, TEntity>
		where TEntity : BaseEntity
	{
		private readonly ISessionRepository _session;

		public EntityConverter(ISessionRepository session)
		{
			_session = session;
		}

		public TEntity Convert(long source, TEntity destination, ResolutionContext context)
		{
			if (source == default)
				return default;

			return _session.LoadAsync<TEntity>(source).Result;
		}

		public TEntity Convert(long? source, TEntity destination, ResolutionContext context)
		{
			if (!source.HasValue || source.Value == default)
				return default;

			return _session.LoadAsync<TEntity>(source.Value).Result;
		}
	}
}