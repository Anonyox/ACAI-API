using FluentValidation.Results;

namespace ACAI_API.Domain.Util.Validation
{
    public class ValidateableError
	{
		public string PropertyName { get; set; }

		public string Message { get; set; }

		public int Severity { get; set; }

		public ValidateableError()
		{
		}

		public ValidateableError(ValidationFailure validationFailure)
		{
			PropertyName = validationFailure.PropertyName;
			Message = validationFailure.ErrorMessage;
			Severity = (int)validationFailure.Severity;
		}
	}
}
