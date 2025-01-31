﻿using GroshieHub.Domain.Exceptions;
using GroshieHub.Domain.Exceptions.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace GroshieHub.Presentation.Extensions;

public sealed class GlobalErrorsHandler : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		var error = context.Exception;

		if (error is not CurrencyNotFoundException)
		{
			Log.Error("Something went wrong: {error}", error);
		}

		var statusCode = error switch
		{
			InvalidRequestException => StatusCodes.Status422UnprocessableEntity,
			NotFoundException => StatusCodes.Status404NotFound,
			ApiRequestLimitException => StatusCodes.Status429TooManyRequests,
			BadRequestException => StatusCodes.Status400BadRequest,
			_ => StatusCodes.Status500InternalServerError,
		};

		string? detail = null;

		if (statusCode == StatusCodes.Status422UnprocessableEntity)
		{
			detail = "For more information, see documentation: https://currencyapi.com/docs/status-codes#_422";
		}
		if (statusCode != StatusCodes.Status404NotFound)
		{
			Log.Error(error.ToString());
		}

		context.Result = new ObjectResult(
			new ProblemDetails
			{
				Status = statusCode,
				Title = error.Message,
				Detail = detail,
			}
		);

		context.ExceptionHandled = true;
	}
}