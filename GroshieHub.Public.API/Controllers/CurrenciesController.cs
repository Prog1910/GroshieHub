using GroshieHub.Public.Core.Services;
using GroshieHub.Public.Shared.DTO;
using GroshieHub.Public.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GroshieHub.Public.API.Controllers;

[Route("api/currencies")]
[ApiController]
public sealed class CurrenciesController(ICurrencyService currencyService) : ControllerBase
{
	[HttpGet()]
	public async Task<ActionResult<CurrencyDto>> GetDefault(CancellationToken token = default)
	{
		var currency = await currencyService.GetDefaultAsync(token);

		return Ok(currency);
	}

	[HttpGet("{code}")]
	public async Task<ActionResult<CurrencyDto>> GetByCode(CurrencyCode code, CancellationToken token = default)
	{
		var currency = await currencyService.GetByCodeAsync(code.ToString(), token);

		return Ok(currency);
	}

	[HttpGet("{code}/{date:datetime}")]
	public async Task<ActionResult<CurrencyOnDateDto>> GetByCodeOnDate(DateTime date, CurrencyCode code, CancellationToken token = default)
	{
		var currency = await currencyService.GetByCodeOnDateAsync(code.ToString(), date, token);

		return Ok(currency);
	}

	[HttpGet("settings")]
	public async Task<ActionResult<CurrencyApiSettingsDto>> GetSettings(CancellationToken token = default)
	{
		var settings = await currencyService.GetSettingsAsync(token);

		return Ok(settings);
	}
}