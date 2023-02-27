using AutoMapper;
using ACAI_API.Domain.Authorization.Model;
//using ACAI_API.Domain.ConnectionTables.Employee.Repositories;
using ACAI_API.Domain.Util.Caching;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Authorization.Services
{
	public class AuthService : IAuthService
	{
		private const string TokensKey = "_tokenList";

		private readonly TimeSpan _tokensExpiresAt = TimeSpan.FromDays(1);
		private readonly AuthSettings _authSettings;
		private readonly ICache _cache;
		private readonly IMapper _mapper;

		//private readonly IEmployeeRepository _employeeRepository;

		public AuthService(IOptions<AuthSettings> authSettings,
			ICache cache,
			//IEmployeeRepository employeeRepository,
			IMapper mapper)
		{
			_authSettings = authSettings.Value;
			_cache = cache;
			//_employeeRepository = employeeRepository;
			_mapper = mapper;
		}

		private List<TokenItem> Tokens
			=> _cache.Fetch(TokensKey, () => new List<TokenItem>(), _tokensExpiresAt, true);

		private void SaveTokens(List<TokenItem> tokens)
		{
			_cache.Set(TokensKey, tokens, _tokensExpiresAt);
		}

		private void SaveNewToken(TokenItem item)
		{
			var tokenList = Tokens.ToList();
			var currentItem = tokenList.FirstOrDefault(x => x.Username == item.Username && !x.Expired);

			if (currentItem != null)
				currentItem.Expired = true;

			tokenList.Add(item);

			SaveTokens(tokenList);
		}

		public void InvalidateToken(string username)
		{
			if (string.IsNullOrWhiteSpace(username))
				return;

			var tokenList = Tokens.ToList();
			var currentItem = tokenList.FirstOrDefault(x => x.Username == username && !x.Expired);

			if (currentItem == null)
				return;

			currentItem.Expired = true;

			SaveTokens(tokenList);
		}

		public bool IsValidToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return true;

			return Tokens.Any(x => x.Token == token && !x.Expired);
		}

		private AuthResponseModel GetNewToken(UserPrincipal userPrincipal)
		{
			var claims = GetClaims(userPrincipal);

			var identityClaims = new ClaimsIdentity(claims);

			var key = Encoding.ASCII.GetBytes(_authSettings.Secret);

			var tokenHandler = new JwtSecurityTokenHandler();

			var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor()
			{
				Issuer = _authSettings.ValidIssuer,
				Audience = _authSettings.ValidAudiance,
				Subject = identityClaims,
				Expires = DateTime.UtcNow.AddHours(_authSettings.ExpiresAt),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			});

			var token = tokenHandler.WriteToken(securityToken);

			var result = new AuthResponseModel
			{
				AccessToken = token,
				ExpiresIn = TimeSpan.FromHours(_authSettings.ExpiresAt).TotalSeconds,
				Employee_Id = userPrincipal.Id,
				Employee_Name = userPrincipal.Name,
				Employee_Username = userPrincipal.UserName,
				//Permissions = userPrincipal.Permissions,
			};

			SaveNewToken(new TokenItem()
			{
				Username = userPrincipal.UserName,
				Token = token,
				ExpiresAt = DateTime.UtcNow.AddHours(_authSettings.ExpiresAt)
			});

			return result;
		}

		private static Claim[] GetClaims(UserPrincipal user)
		{
			var claims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
			};

			//foreach (var role in user.Roles)
			//	claims.Add(new Claim("role", role));

			 claims.Add(new Claim("user", JsonSerializer.Serialize(user)));

			return claims.ToArray();
		}

		public UserPrincipal GetUser(IPrincipal principal)
		{
			var claimsPrincipal = principal as ClaimsPrincipal;
			if (claimsPrincipal == null)
				return null;

			var value = claimsPrincipal.FindFirst("user")?.Value;
			if (string.IsNullOrWhiteSpace(value))
				return null;

			return JsonSerializer.Deserialize<UserPrincipal>(value);
		}

		/*
		public async Task<AuthResponseModel> AuthenticateUser(LoginModel loginModel)
		{
			var employee = await _employeeRepository.GetEmployeeForAuthentication(loginModel.Username, loginModel.Password);

			if (employee == null)
				return null;

			var userPrincipal = new UserPrincipal()
			{
				Id = employee.Id,
				Name = employee.Person.Name,
				UserName = employee.UserName,
				Permissions = _mapper.Map<PermissionsDto>(employee.Permissions)

			};

			var authResponseModel = GetNewToken(userPrincipal);

			return authResponseModel;
		}
		*/
	}
}
