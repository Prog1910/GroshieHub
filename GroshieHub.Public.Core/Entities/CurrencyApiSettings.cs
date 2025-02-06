using GroshieHub.Public.Shared.Common.Settings;

namespace GroshieHub.Public.Core.Entities;

public sealed class CurrencyApiSettings : ApiSettingsBase
{
	private string _baseCurrencyCode = string.Empty;
	private string _defaultCurrencyCode = string.Empty;

	public override string ApiKeyHeader => "apikey";

	public string BaseCurrencyCode
	{
		get => _baseCurrencyCode;
		set => _baseCurrencyCode = value.ToUpperInvariant();
	}

	public string DefaultCurrencyCode
	{
		get => _defaultCurrencyCode;
		set => _defaultCurrencyCode = value.ToUpperInvariant();
	}

	public int CurrencyRoundCount { get; set; }
}