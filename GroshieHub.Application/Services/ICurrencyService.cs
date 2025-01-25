using GroshieHub.Domain.Entities;
using GroshieHub.Shared.DTO;

namespace GroshieHub.Application.Services;

public interface ICurrencyService
{
	Task<CurrencyDto> GetDefaultAsync(CancellationToken token = default);

	Task<CurrencyDto> GetByCodeAsync(ECurrencyCode code, CancellationToken token = default);

	Task<CurrencyOnDateDto> GetByCodeOnDateAsync(ECurrencyCode code, DateTime date, CancellationToken token = default);

	Task<CurrencyApiSettingsDto> GetSettingsAsync(CancellationToken token = default);
}