using GroshieHub.Application.Services;
using GroshieHub.Domain.Entities;
using GroshieHub.Domain.Exceptions;
using GroshieHub.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GroshieHub.API.Controllers;

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
	public async Task<ActionResult<CurrencyDto>> GetByCode(ECurrencyCode code = ECurrencyCode.UNKNOWN, CancellationToken token = default)
	{
		ValidateCode(code);
		var currency = await currencyService.GetByCodeAsync(code, token);

		return Ok(currency);
	}

	[HttpGet("{code}/{date:datetime}")]
	public async Task<ActionResult<CurrencyOnDateDto>> GetByCodeOnDate(DateTime date, ECurrencyCode code = ECurrencyCode.UNKNOWN, CancellationToken token = default)
	{
		ValidateCode(code);
		var currency = await currencyService.GetByCodeOnDateAsync(code, date, token);

		return Ok(currency);
	}

	[HttpGet("settings")]
	public async Task<ActionResult<CurrencyApiSettingsDto>> GetSettings(CancellationToken token = default)
	{
		var settings = await currencyService.GetSettingsAsync(token);

		return Ok(settings);
	}

	private static void ValidateCode(ECurrencyCode code)
	{
		if (code == ECurrencyCode.UNKNOWN)
			throw new UnspecifiedCurrencyException();
	}
}