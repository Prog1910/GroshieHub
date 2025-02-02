using GroshieHub.Domain.Common.Services;
using GroshieHub.Domain.Exceptions;
using GroshieHub.Domain.Settings;
using GroshieHub.Shared.DTO;
using GroshieHub.Shared.Extensions;
using Mapster;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace GroshieHub.Application.Services;

public sealed class CurrencyService : ServiceBase, ICurrencyService
{
	private readonly CurrencyApiSettings _settings;

	public CurrencyService(HttpClient client, IOptionsSnapshot<CurrencyApiSettings> settings)
		: base(client)
	{
		_settings = settings.Value;
		SetupClient();
	}

	protected override void SetupBaseAddress() => Client.BaseAddress = new Uri(_settings.BaseUrl);

	protected override void SetupApiKey() => Client.DefaultRequestHeaders.Add("apikey", _settings.ApiKey);

	public async Task<CurrencyDto> GetDefaultAsync(CancellationToken token = default)
	{
		var code = _settings.DefaultCurrencyCode;
		var rate = await GetExchangeRateAsync(code, token: token);

		return (new { Code = code, Rate = FormatRate(rate) }).Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetByCodeAsync(string code, CancellationToken token = default)
	{
		var rate = await GetExchangeRateAsync(code, token: token);

		return (new { Code = code, Rate = FormatRate(rate) }).Adapt<CurrencyDto>();
	}

	public async Task<CurrencyOnDateDto> GetByCodeOnDateAsync(string code, DateTime date, CancellationToken token = default)
	{
		var rate = await GetExchangeRateAsync(code, date, token);

		return (new
		{
			Date = FormatDate(date),
			Code = code,
			Rate = FormatRate(rate),
		}).Adapt<CurrencyOnDateDto>();
	}

	public async Task<CurrencyApiSettingsDto> GetSettingsAsync(CancellationToken token = default)
	{
		var (total, used) = await GetRequestLimitInfoAsync(token);

		var settings = _settings.Adapt<CurrencyApiSettingsDto>();
		settings.RequestLimit = total;
		settings.RequestCount = used;

		return settings;
	}

	private async Task EnsureRequestLimitNotExceeded(CancellationToken token = default)
	{
		var (total, used) = await GetRequestLimitInfoAsync(token);

		if (int.IsNegative(total - used))
		{
			throw new ApiRequestLimitException();
		}
	}

	private async Task<decimal> GetExchangeRateAsync(string code, DateTime? date = null, CancellationToken token = default)
	{
		await EnsureRequestLimitNotExceeded(token);

		var uri = date == null
				? $"{Client.BaseAddress}latest?currencies={code}&base_currency={_settings.BaseCurrencyCode}"
				: $"{Client.BaseAddress}historical?currencies={code}&date={date}&base_currency={_settings.BaseCurrencyCode}";

		var rootElement = await Client.GetFromJsonAsync<JsonElement>(uri, token);

		rootElement.GetProperty($"data.{code}.value", out decimal rate);

		return rate;
	}

	private async Task<(short Total, short Used)> GetRequestLimitInfoAsync(CancellationToken token = default)
	{
		var uri = $"{Client.BaseAddress}status";

		var rootElement = await Client.GetFromJsonAsync<JsonElement>(uri, token);
		rootElement.GetProperty("quotas.month", out JsonElement monthElement);
		monthElement.GetProperty("total", out short total);
		monthElement.GetProperty("used", out short used);

		return (total, used);
	}

	private decimal FormatRate(decimal rate) => rate.RoundToDecimalPlaces(_settings.CurrencyRoundCount);

	private static string FormatDate(DateTime date) => date.FormatDateTime("yyyy-MM-dd");
}