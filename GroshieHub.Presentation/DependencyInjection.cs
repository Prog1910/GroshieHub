using Audit.Core;
using Audit.Http;
using GroshieHub.Domain.Settings;
using GroshieHub.Domain.Settings.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace GroshieHub.Presentation;

public static class DependencyInjection
{
	public static IServiceCollection AddPresentation(this IServiceCollection services)
	{
		services.ConfigureCors();
		services.ConfigureSerilog();
		services.ConfigureSwagger();
		services.ConfigureApiSettings();
		services.ConfigureFilterAttributes();

		return services;
	}

	private static void ConfigureApiSettings(this IServiceCollection services)
	{
		services.AddOptionsWithValidateOnStart<CurrencyApiSettings, CurrencyApiSettingsValidator>()
					.BindConfiguration(nameof(CurrencyApiSettings));
	}

	private static void ConfigureSerilog(this IServiceCollection _)
	{
		Configuration
			.Setup()
			.UseSerilog(config =>
				config.Message(auditEvent =>
				{
					if (auditEvent is not AuditEventHttpClient httpClientEvent)
						return auditEvent.ToJson();

					var contentBody = httpClientEvent.Action?.Response?.Content?.Body;
					if (contentBody is not string { Length: > 1000 } stringBody)
						return auditEvent.ToJson();

					var responseContent = httpClientEvent.Action!.Response?.Content;
					if (responseContent is not null)
						responseContent.Body = stringBody[..1000] + "<...>";
					return auditEvent.ToJson();
				})
			);
	}

	private static void ConfigureCors(this IServiceCollection services)
	{
		services.AddCors(options
			=> options.AddPolicy("CorsPolicy", static builder
				=> builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Pagination")
			));
	}

	private static void ConfigureSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Groshie Hub Public API" });

			var xmlPath = $"{typeof(Program).Assembly.GetName().Name}.xml";
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlPath));
		});
	}

	private static void ConfigureFilterAttributes(this IServiceCollection services)
	{
		services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
	}
}