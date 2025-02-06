using GroshieHub.Public.Core.Entities;
using GroshieHub.Public.Shared.DTO;
using GroshieHub.Public.Shared.Extensions;
using Mapster;
using Microsoft.Extensions.Options;

namespace GroshieHub.Public.Core.Services;

public sealed class CurrencyService : ICurrencyService
{
	private readonly ICurrencyClient _currencyClient;
	private readonly CurrencyApiSettings _settings;

	public CurrencyService(ICurrencyClient currencyClient, IOptionsSnapshot<CurrencyApiSettings> settings)
	{
		_currencyClient = currencyClient;
		_settings = settings.Value;
	}

	public async Task<CurrencyDto> GetDefaultAsync(CancellationToken token = default)
	{
		var code = _settings.DefaultCurrencyCode;
		var rate = await _currencyClient.GetExchangeRateAsync(code, token: token);

		return (new { Code = code, Rate = FormatRate(rate) }).Adapt<CurrencyDto>();
	}

	public async Task<CurrencyDto> GetByCodeAsync(string code, CancellationToken token = default)
	{
		var rate = await _currencyClient.GetExchangeRateAsync(code, token: token);

		return (new { Code = code, Rate = FormatRate(rate) }).Adapt<CurrencyDto>();
	}

	public async Task<CurrencyOnDateDto> GetByCodeOnDateAsync(string code, DateTime date, CancellationToken token = default)
	{
		var rate = await _currencyClient.GetExchangeRateAsync(code, date, token);

		return (new
		{
			Date = FormatDate(date),
			Code = code,
			Rate = FormatRate(rate),
		}).Adapt<CurrencyOnDateDto>();
	}

	public async Task<CurrencyApiSettingsDto> GetSettingsAsync(CancellationToken token = default)
	{
		var (total, used) = await _currencyClient.GetRequestLimitInfoAsync(token);

		var settings = _settings.Adapt<CurrencyApiSettingsDto>();
		settings.RequestLimit = total;
		settings.RequestCount = used;

		return settings;
	}

	private decimal FormatRate(decimal rate) => rate.RoundToDecimalPlaces(_settings.CurrencyRoundCount);

	private string FormatDate(DateTime date) => date.FormatDateTime("yyyy-MM-dd");
}