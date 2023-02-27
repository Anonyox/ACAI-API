using System;
using Microsoft.AspNetCore.Mvc;

namespace ACAI_API.Api.Util.Filters
{
	public class AsyncTransactionAttribute : ServiceFilterAttribute
	{
		public AsyncTransactionAttribute()
			: base(typeof(AsyncTransactionActionFilter))
		{
		}

		public AsyncTransactionAttribute(Type type)
			: base(type)
		{
		}
	}
}