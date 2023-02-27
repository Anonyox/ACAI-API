using FluentValidation.Results;
using System.Collections.ObjectModel;

namespace ACAI_API.Domain.Util.Validation
{
    /// <summary>
    /// Response model including validation.
    /// </summary>
    public class ValidateableResponse
	{
		private readonly IList<ValidateableError> _errorMessages;

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public ValidateableResponse()
		{
			_errorMessages = new List<ValidateableError>();
		}

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		/// <param name="validationResult"></param>
		public ValidateableResponse(ValidationResult validationResult)
		{
			_errorMessages = validationResult.Errors
				.Select(validationFailure => new ValidateableError(validationFailure))
				.ToList();
		}

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		/// <param name="errors"></param>
		public ValidateableResponse(IList<ValidateableError> errors)
		{
			_errorMessages = errors;
		}

		/// <summary>
		/// Indicates that there are no validation errors.
		/// </summary>
		public bool Success => _errorMessages.Count == 0;

		/// <summary>
		/// Validation errors.
		/// </summary>
		public IReadOnlyCollection<ValidateableError> Errors => new ReadOnlyCollection<ValidateableError>(_errorMessages);
	}

	/// <summary>
	/// Generic response model including validation
	/// </summary>
	public class ValidateableResponse<TModel> : ValidateableResponse
		where TModel : class
	{
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		/// <param name="model"></param>
		public ValidateableResponse(TModel model)
		{
			Model = model;
		}

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="validationErrors"></param>
		public ValidateableResponse(ValidationResult validationResult)
			: base(validationResult)
		{
		}

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="validationResult"></param>
		public ValidateableResponse(TModel model, ValidationResult validationResult)
			: base(validationResult)
		{
			Model = model;
		}

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="validationErrors"></param>
		public ValidateableResponse(IList<ValidateableError> validationErrors)
			: base(validationErrors)
		{
		}

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="validationErrors"></param>
		public ValidateableResponse(TModel model, IList<ValidateableError> validationErrors)
			: base(validationErrors)
		{
			Model = model;
		}

		/// <summary>
		/// Generic result.
		/// </summary>
		public TModel Model { get; }
	}
}
