using System;

namespace ACAI_API.Domain.Authorization.Model
{
	public class AuthResponseModel
	{
		public string? AccessToken { get; set; }

		public double ExpiresIn { get; set; }

        public Guid Employee_Id { get; set; }

        public string? Employee_Name { get; set; }

        public string? Employee_Username { get; set; }

        

    }
}
