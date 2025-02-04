using GroshieHub.Modules.Currencies.Shared.DTO;

namespace GroshieHub.Modules.Currencies.Core.Services;

public interface ICurrencyService
{
	Task<CurrencyDto> GetDefaultAsync(CancellationToken token = default);

	Task<CurrencyDto> GetByCodeAsync(string code, CancellationToken token = default);

	Task<CurrencyOnDateDto> GetByCodeOnDateAsync(string code, DateTime date, CancellationToken token = default);

	Task<CurrencyApiSettingsDto> GetSettingsAsync(CancellationToken token = default);
}