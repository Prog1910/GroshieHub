namespace GroshieHub.Public.Shared.Common.Settings;

public abstract class ApiSettingsBase
{
	public abstract string ApiKeyHeader { get; }
	public string BaseUrl { get; set; } = string.Empty;
	public string ApiKey { get; set; } = string.Empty;
}