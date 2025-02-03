namespace GroshieHub.Application.Services;

public interface ICurrencyClient
{
	Task<decimal> GetExchangeRateAsync(string code, DateTime? date = null, CancellationToken token = default);

	Task<(short Total, short Used)> GetRequestLimitInfoAsync(CancellationToken token = default);
}