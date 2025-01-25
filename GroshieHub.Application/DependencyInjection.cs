using Audit.Http;
using GroshieHub.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace GroshieHub.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.ConfigureCurrencyHttpClient();
		services.ConfigureServices();

		return services;
	}

	private static void ConfigureServices(this IServiceCollection services)
	{
		services.AddScoped<ICurrencyService, CurrencyService>();
	}

	private static void ConfigureCurrencyHttpClient(this IServiceCollection services)
	{
		services.AddHttpClient<ICurrencyService, CurrencyService>("CurrencyClient")
			.AddPolicyHandler(
				HttpPolicyExtensions
					.HandleTransientHttpError()
					.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) - 1))
			)
			.AddAuditHandler(audit =>
				audit
					.IncludeRequestHeaders()
					.IncludeRequestBody()
					.IncludeResponseHeaders()
					.IncludeResponseBody()
					.IncludeContentHeaders()
			);
	}
}