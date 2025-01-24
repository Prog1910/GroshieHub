using GroshieHub.Domain.Abstractions.Entities.Validators;
using GroshieHub.Domain.Extensions;
using Microsoft.Extensions.Options;

namespace GroshieHub.Domain.Entities.Validators;

public sealed class CurrencyApiSettingsValidator : ApiSettingsBaseValidator<CurrencyApiSettings>
{
	public override ValidateOptionsResult Validate(string? name, CurrencyApiSettings options)
	{
		if (base.Validate(name, options) is var baseValidationResult && baseValidationResult.Failed)
		{
			return baseValidationResult;
		}

		var validationResultBuilder = new ValidateOptionsResultBuilder();

		validationResultBuilder.AddResult(base.Validate(name, options));

		if (options.BaseCurrencyCode.IsInvalid())
		{
			validationResultBuilder.AddError($"Base currency code is invalid.");
		}

		if (options.DefaultCurrencyCode.IsInvalid())
		{
			validationResultBuilder.AddError($"Default currency code is invalid.");
		}

		if (int.IsNegative(options.CurrencyRoundCount))
		{
			validationResultBuilder.AddError("Decimal places of the exchange rate can't a be negative number.");
		}

		return validationResultBuilder.Build();
	}
}