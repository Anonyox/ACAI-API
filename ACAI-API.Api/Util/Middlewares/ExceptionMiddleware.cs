using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using ACAI_API.Api.Util.Handlers;

namespace ACAI_API.Api.Util.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			if (exception is ValidationException validationException)
			{
				var errors = validationException.Errors.Select(e => new
				{
					property = e.PropertyName,
					message = e.ErrorMessage,
					severity = e.Severity
				}).ToList();

				var problemDetails = new ProblemDetails()
				{
					Type = "Unprocessable Entity",
					Title = "422 Unprocessable Entity",
					Status = StatusCodes.Status422UnprocessableEntity,
					Instance = context.Request.Path,
					Detail = errors,
				};

				var jsonErrors = JsonSerializer.Serialize(problemDetails);

				context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
				await context.Response.WriteAsync(jsonErrors);

				return;
			}

			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsync(exception.Message);
		}
	}
}