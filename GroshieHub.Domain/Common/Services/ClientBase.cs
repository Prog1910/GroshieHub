using GroshieHub.Domain.Common.Settings;
using Microsoft.Extensions.Options;

namespace GroshieHub.Domain.Common.Services;

public abstract class ClientBase
{
	private readonly HttpClient _client;
	public HttpClient Client => _client;

	public ClientBase(HttpClient client) => _client = client;

	protected void SetupClient()
	{
		SetupBaseAddress();
		SetupApiKey();
	}

	protected abstract void SetupBaseAddress();

	protected abstract void SetupApiKey();
}

public abstract class ClientBase<TSettings> : ClientBase where TSettings : ApiSettingsBase
{
	protected readonly TSettings _settings;

	public ClientBase(HttpClient client, IOptionsSnapshot<TSettings> settings)
		: base(client)
		=> _settings = settings.Value;

	protected override void SetupBaseAddress() => Client.BaseAddress = new Uri(_settings.BaseUrl);

	protected override void SetupApiKey() => Client.DefaultRequestHeaders.Add(_settings.ApiKeyHeader, _settings.ApiKey);
}