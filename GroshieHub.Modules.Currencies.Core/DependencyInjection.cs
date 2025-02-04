using Audit.Http;
using GroshieHub.Modules.Currencies.Core.Entities;
using GroshieHub.Modules.Currencies.Core.Entities.Validators;
using GroshieHub.Modules.Currencies.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace GroshieHub.Modules.Currencies.Core;

public static class DependencyInjection
{
	public static IServiceCollection AddCore(this IServiceCollection services)
	{
		services.ConfigureApiSettings();
		services.ConfigureCurrencyHttpClient();
		services.ConfigureServices();

		return services;
	}

	private static void ConfigureServices(this IServiceCollection services)
	{
		services.AddScoped<ICurrencyClient, CurrencyClient>();
		services.AddScoped<ICurrencyService, CurrencyService>();
	}

	private static void ConfigureApiSettings(this IServiceCollection services)
	{
		services.AddOptionsWithValidateOnStart<CurrencyApiSettings, CurrencyApiSettingsValidator>()
					.BindConfiguration(nameof(CurrencyApiSettings));
	}

	private static void ConfigureCurrencyHttpClient(this IServiceCollection services)
	{
		services.AddHttpClient<ICurrencyClient, CurrencyClient>("CurrencyClient")
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