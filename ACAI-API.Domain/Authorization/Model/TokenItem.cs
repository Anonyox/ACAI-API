using System;

namespace ACAI_API.Domain.Authorization.Model
{
	internal class TokenItem
	{
		public string Username { get; set; }
		public string Token { get; set; }
		public DateTime ExpiresAt { get; set; }
		public bool Expired { get; set; }
	}
}
