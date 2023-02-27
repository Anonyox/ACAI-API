using ACAI_API.Domain.Authorization.Model;
using ACAI_API.Domain.Authorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ACAI_API.Api.Controllers
{
	[Route("auth")]
	public class AuthController : BaseController
	{
		private readonly IAuthService _authService = null;
		private readonly IServiceProvider _serviceProvider;
		private readonly AuthSettings _authSettings;

		public AuthController(IAuthService authService, IServiceProvider serviceProvider, IOptions<AuthSettings> authSettings)
		{
			_authService = authService;
			_serviceProvider = serviceProvider;
			_authSettings = authSettings.Value;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (model == null)
				return BadRequest(new { errorMessage = "Você deve prover um usuário e senha no formato correto." });

			var authToken = await _authService.AuthenticateUser(model);

			if (authToken == null)
				return UnprocessableEntity(new { errorMessage = "Usuário ou senha estão incorretos." });
			
			return Ok(authToken);
		}

		[Authorize]
		[HttpGet("logout")]
		public IActionResult Logout()
		{
			var userPrincipal = this._serviceProvider.GetService<UserPrincipal>();

			_authService.InvalidateToken(userPrincipal?.UserName);

			return Ok();
		}

		[Authorize]
		[HttpGet("ping")]
		public IActionResult Ping() => Ok();
	}
}