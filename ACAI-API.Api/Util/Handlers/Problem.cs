namespace ACAI_API.Api.Util.Handlers
{
	public static class ProblemConstants
	{
		public static readonly string InternalServerErrorTitle = "An error occurred while processing your request.";
		public static readonly string InternalServerErrorType = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
		public static readonly string NotFoundTitle = "The specified resource was not found.";
		public static readonly string NotFoundType = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
		public static readonly string BadRequestTitle = "Invalid request.";
		public static readonly string BadRequestType = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
		public static readonly string UnprocessableEntityTitle = "Unprocessable entity.";
		public static readonly string UnprocessableEntityType = "https://tools.ietf.org/html/rfc7231#section-6.5";
	}
}
