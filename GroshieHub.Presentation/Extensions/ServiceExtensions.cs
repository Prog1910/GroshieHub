using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

namespace GroshieHub.Presentation.Extensions;

public static class ServiceExtensions
{
	public static void SetupSerilog(this WebApplicationBuilder builder)
	{
		builder.Host.UseSerilog((context, _, configuration)
			=> configuration.ReadFrom.Configuration(context.Configuration).Enrich
				.WithMachineName().Enrich
				.FromLogContext().Enrich
				.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers())
		);
	}

	public static void SetupSwagger(this IApplicationBuilder app)
	{
		app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			options.RoutePrefix = "swagger";
		});
	}

	public static void SetupReDoc(this IApplicationBuilder app)
	{
		app.UseReDoc(options => options.RoutePrefix = string.Empty);
	}
}