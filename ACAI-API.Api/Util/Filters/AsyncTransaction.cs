using ACAI_API.Domain.Util.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ACAI_API.Api.Util.Filters
{
	// References:
	// Required reading: https://docs.microsoft.com/en-US/ef/core/saving/transactions
	// TLTR: https://entityframeworkcore.com/saving-data-transaction

	public class AsyncTransactionActionFilter : IAsyncActionFilter
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<AsyncTransactionActionFilter> _logger;

		public AsyncTransactionActionFilter(IUnitOfWork unitOfWork,
											ILogger<AsyncTransactionActionFilter> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			_unitOfWork.BeginTransaction();

			var actionResult = await next();

			if (actionResult.Exception == null)
			{
				if (IsValidResponse(actionResult.Result))
				{
					try
					{
						await _unitOfWork.CommitAsync();

						return;
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "An error occurred while trying to commit the transaction.");

						await _unitOfWork.RollbackAsync();

						throw ex;
					}
				}
				else
				{
					_logger.LogError(actionResult.Exception, "The request does not return an valid http response. See the response for the details.");

					await _unitOfWork.RollbackAsync();
				}
			}
			else
			{
				_logger.LogError(actionResult.Exception, "An error occurred while trying to execute. See the exception for details.");

				await _unitOfWork.RollbackAsync();

				throw actionResult.Exception;
			}
		}

		private bool IsValidResponse(IActionResult actionResult)
		{
			return actionResult is OkResult ||
				actionResult is CreatedAtActionResult ||
				actionResult is CreatedResult ||
				actionResult is NoContentResult;
		}
	}
}