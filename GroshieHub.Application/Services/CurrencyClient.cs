using GroshieHub.Domain.Common.Services;
using GroshieHub.Domain.Exceptions;
using GroshieHub.Domain.Settings;
using GroshieHub.Shared.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace GroshieHub.Application.Services;

public sealed class CurrencyClient : ClientBase<CurrencyApiSettings>, ICurrencyClient
{
	public CurrencyClient(HttpClient client, IOptionsSnapshot<CurrencyApiSettings> settings)
		: base(client, settings)
	{
		SetupClient();
	}

	public async Task<decimal> GetExchangeRateAsync(string code, DateTime? date = null, CancellationToken token = default)
	{
		await EnsureRequestLimitNotExceeded(token);

		var uri = date == null
				? $"{Client.BaseAddress}latest?currencies={code}&base_currency={_settings.BaseCurrencyCode}"
				: $"{Client.BaseAddress}historical?currencies={code}&date={date}&base_currency={_settings.BaseCurrencyCode}";

		var rootElement = await Client.GetFromJsonAsync<JsonElement>(uri, token);

		rootElement.GetProperty($"data.{code}.value", out decimal rate);

		return rate;
	}

	public async Task<(short Total, short Used)> GetRequestLimitInfoAsync(CancellationToken token = default)
	{
		var uri = $"{Client.BaseAddress}status";

		var rootElement = await Client.GetFromJsonAsync<JsonElement>(uri, token);
		rootElement.GetProperty("quotas.month", out JsonElement monthElement);
		monthElement.GetProperty("total", out short total);
		monthElement.GetProperty("used", out short used);

		return (total, used);
	}

	private async Task EnsureRequestLimitNotExceeded(CancellationToken token = default)
	{
		var (total, used) = await GetRequestLimitInfoAsync(token);

		if (int.IsNegative(total - used)) throw new ApiRequestLimitException();
	}
}