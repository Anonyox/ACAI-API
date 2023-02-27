using ACAI_API.Api.Util.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ACAI_API.Api.Util.Extensions
{
	public static class AppExtensions
	{
		public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder app)
		{
			return app.UseMiddleware<UserTokenMiddleware>();
		}

		public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
		{
			return app.UseMiddleware<ExceptionMiddleware>();
		}
	}
}