using GroshieHub.Domain.Settings;
using GroshieHub.Domain.Settings.Validators;

namespace GroshieHub.Domain.Tests;

public sealed class CurrencyApiSettingsSpecs
{
	private static readonly CurrencyApiSettingsValidator _validator = new();

	[Theory]
	[InlineData("usd", 2, "USD")]
	[InlineData("usD", 5, "USD")]
	[InlineData("USD", 10, "USD")]
	public void Can_create_settings_with_valid_args(string currencyCode, int roundCount, string expectedCode)
	{
		var settings = new CurrencyApiSettings
		{
			BaseCurrencyCode = currencyCode,
			DefaultCurrencyCode = currencyCode,
			CurrencyRoundCount = roundCount
		};

		Assert.NotNull(settings);
		Assert.Equal(expectedCode, settings.BaseCurrencyCode);
		Assert.Equal(expectedCode, settings.DefaultCurrencyCode);
		Assert.Equal(roundCount, settings.CurrencyRoundCount);
	}

	[Theory]
	[InlineData("DDD")]
	[InlineData("")]
	public void Fail_validation_with_unknown_codes(string unknownCode)
	{
		var settings = new CurrencyApiSettings
		{
			BaseUrl = "www.example.com",
			ApiKey = "SECRET_KEY",
			BaseCurrencyCode = unknownCode,
			DefaultCurrencyCode = unknownCode
		};

		var validationResult = _validator.Validate(nameof(CurrencyApiSettings), settings);

		Assert.NotNull(validationResult);
		Assert.True(validationResult.Failed);
		Assert.Contains($"Base currency code '{unknownCode}' is invalid.", validationResult.Failures);
		Assert.Contains($"Default currency code '{unknownCode}' is invalid.", validationResult.Failures);
	}

	[Fact]
	public void Fail_validation_with_negative_round_cound()
	{
		var settings = new CurrencyApiSettings
		{
			BaseUrl = "www.example.com",
			ApiKey = "SECRET_KEY",
			CurrencyRoundCount = -1
		};

		var validationResult = _validator.Validate(nameof(CurrencyApiSettings), settings);

		Assert.NotNull(validationResult);
		Assert.True(validationResult.Failed);
		Assert.Contains("Decimal places of the exchange rate can't a be negative number.", validationResult.Failures);
	}
}