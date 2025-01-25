using GroshieHub.Domain.Abstractions.Entities;

namespace GroshieHub.Domain.Entities;

public sealed class CurrencyApiSettings : ApiSettingsBase
{
	public ECurrencyCode BaseCurrencyCode { get; set; } = ECurrencyCode.UNKNOWN;
	public ECurrencyCode DefaultCurrencyCode { get; set; } = ECurrencyCode.UNKNOWN;
	public int CurrencyRoundCount { get; set; }
}