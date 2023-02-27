using ACAI_API.Domain.Authorization.Model;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Authorization.Services
{
	public interface IAuthService
	{
		//Task<AuthResponseModel> AuthenticateUser(LoginModel loginModel);

		bool IsValidToken(string token);

		void InvalidateToken(string username);

		UserPrincipal GetUser(IPrincipal principal);
	}
}
