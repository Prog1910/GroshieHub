using System.Text.Json.Serialization;
using GroshieHub.Public.Core;
using GroshieHub.Public.Presentation;
using GroshieHub.Public.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

{
	builder.SetupSerilog();

	builder.Services
		.AddCore()
		.AddPresentation();

	builder
		.Services.AddControllers(static config => config.Filters.Add<GlobalErrorsHandler>())
		.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
}

var app = builder.Build();

{
	app.UseMiddleware<RequestLoggingMiddleware>();

	if (app.Environment.IsDevelopment()) app.SetupSwagger();
	else app.SetupReDoc();

	app.UseCors("CorsPolicy");

	app.UseHttpsRedirection();
	app.MapControllers();
	app.Run();
}