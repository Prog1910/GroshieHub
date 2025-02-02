namespace GroshieHub.Shared.DTO;

public sealed record CurrencyApiSettingsDto
{
	public string DefaultCurrencyCode { get; set; } = string.Empty;
	public string BasCurrencyCode { get; set; } = string.Empty;
	public int RequestLimit { get; set; }
	public int RequestCount { get; set; }
	public int CurrencyRoundCount { get; set; }
}