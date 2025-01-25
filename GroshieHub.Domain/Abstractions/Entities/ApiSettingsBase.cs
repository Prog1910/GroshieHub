namespace GroshieHub.Domain.Abstractions.Entities;

public abstract class ApiSettingsBase
{
	public string BaseUrl { get; set; } = string.Empty;
	public string ApiKey { get; set; } = string.Empty;
}