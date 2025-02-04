using GroshieHub.Shared.Common.Settings.Validators;
using GroshieHub.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace GroshieHub.Modules.Currencies.Core.Entities.Validators;

public sealed class CurrencyApiSettingsValidator : ApiSettingsBaseValidator<CurrencyApiSettings>
{
	public override ValidateOptionsResult Validate(string? name, CurrencyApiSettings options)
	{
		var validationResultBuilder = new ValidateOptionsResultBuilder();
		validationResultBuilder.AddResult(base.Validate(name, options));

		if (!options.BaseCurrencyCode.Exists())
		{
			validationResultBuilder.AddError($"Base currency code '{options.BaseCurrencyCode}' is invalid.");
		}

		if (!options.DefaultCurrencyCode.Exists())
		{
			validationResultBuilder.AddError($"Default currency code '{options.DefaultCurrencyCode}' is invalid.");
		}

		if (int.IsNegative(options.CurrencyRoundCount))
		{
			validationResultBuilder.AddError("Decimal places of the exchange rate can't a be negative number.");
		}

		return validationResultBuilder.Build();
	}
}