using ACAI_API.Api.Util.Extensions;
using ACAI_API.Domain.Authorization.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;


namespace ACAI_API.Api.Util.Middlewares
{
	public class UserTokenMiddleware
	{
		private readonly RequestDelegate _next;

		public UserTokenMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			var authorizationHeader = httpContext.Request.Headers["authorization"];

			var token = authorizationHeader == StringValues.Empty
				? string.Empty
				: authorizationHeader.First().Split(" ").Last();

			var authService = httpContext.RequestServices.GetService<IAuthService>();
			

			if (authService.IsValidToken(token))
			{
				await _next.Invoke(httpContext);

				return;
			}

			

			httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
		}


	}
}