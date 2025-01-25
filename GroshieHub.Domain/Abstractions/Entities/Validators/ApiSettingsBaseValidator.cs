using Microsoft.Extensions.Options;

namespace GroshieHub.Domain.Abstractions.Entities.Validators;

public abstract class ApiSettingsBaseValidator<TSettings> : IValidateOptions<TSettings>
	where TSettings : ApiSettingsBase
{
	public virtual ValidateOptionsResult Validate(string? name, TSettings options)
	{
		var validationResultBuilder = new ValidateOptionsResultBuilder();

		if (string.IsNullOrWhiteSpace(options.ApiKey))
		{
			validationResultBuilder.AddError("API key is empty.");
		}

		if (string.IsNullOrWhiteSpace(options.BaseUrl))
		{
			validationResultBuilder.AddError("Base URL is empty.");
		}

		return validationResultBuilder.Build();
	}
}