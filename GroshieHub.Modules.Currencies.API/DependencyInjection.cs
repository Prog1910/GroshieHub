using GroshieHub.Modules.Currencies.Core;
using Microsoft.Extensions.DependencyInjection;

namespace GroshieHub.Modules.Currencies.API;

public static class DependencyInjection
{
	public static IServiceCollection AddCurrenciesModule(this IServiceCollection services)
	{
		services.AddCore();

		return services;
	}
}