namespace ACAI_API.Domain.Authorization.Model
{
	public class AuthSettings
	{
		public string? Secret { get; set; }

		public int ExpiresAt { get; set; }

		public string? ValidIssuer { get; set; }

		public string? ValidAudiance { get; set; }
	}
}
