using GroshieHub.Domain.Settings;
using GroshieHub.Domain.Settings.Validators;

namespace GroshieHub.Domain.Tests;

public class CurrencyApiSettingsSpecs
{
	[Theory]
	[InlineData("usd", 2, "USD")]
	[InlineData("usD", 5, "USD")]
	[InlineData("USD", 10, "USD")]
	public void Can_create_settings_with_valid_args(string knownCode, int roundCount, string expectedCode)
	{
		var settings = CurrencyApiSettingsMock.Create(baseCode: knownCode, defaultCode: knownCode, roundCount);

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
		var validator = new CurrencyApiSettingsValidator();
		var settings = CurrencyApiSettingsMock.Create();
		settings.BaseCurrencyCode = unknownCode;
		settings.DefaultCurrencyCode = unknownCode;

		var validationResult = validator.Validate(nameof(CurrencyApiSettings), settings);

		Assert.NotNull(validationResult);
		Assert.True(validationResult.Failed);
		Assert.Contains($"Base currency code '{unknownCode}' is invalid.", validationResult.Failures);
		Assert.Contains($"Default currency code '{unknownCode}' is invalid.", validationResult.Failures);
	}

	[Fact]
	public void Fail_validation_with_negative_round_cound()
	{
		var validator = new CurrencyApiSettingsValidator();
		var settings = CurrencyApiSettingsMock.Create();
		settings.CurrencyRoundCount = -1;

		var validationResult = validator.Validate(nameof(CurrencyApiSettings), settings);

		Assert.NotNull(validationResult);
		Assert.True(validationResult.Failed);
		Assert.Contains("Decimal places of the exchange rate can't a be negative number.", validationResult.Failures);
	}

	private class CurrencyApiSettingsMock
	{
		public static CurrencyApiSettings Create() => new()
		{
			BaseUrl = "www.example.com",
			ApiKey = "SECRET_KEY"
		};

		public static CurrencyApiSettings Create(string baseCode, string defaultCode, int roundCoude)
		{
			var settings = Create();
			settings.BaseCurrencyCode = baseCode;
			settings.DefaultCurrencyCode = defaultCode;
			settings.CurrencyRoundCount = roundCoude;
			return settings;
		}
	}
}