namespace GroshieHub.Domain.Abstractions.Services;

public abstract class ServiceBase(HttpClient client)
{
	protected readonly HttpClient Client = client;

	protected void SetupClient()
	{
		SetupBaseAddress();
		SetupApiKey();
	}

	protected abstract void SetupBaseAddress();

	protected abstract void SetupApiKey();
}