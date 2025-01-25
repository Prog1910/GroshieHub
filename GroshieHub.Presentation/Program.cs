using GroshieHub.Application;
using GroshieHub.Presentation;
using GroshieHub.Presentation.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

{
	builder.SetupSerilog();

	builder.Services
		.AddApplication()
		.AddPresentation();

	builder
		.Services.AddControllers(static config => config.Filters.Add<GlobalErrorsHandler>())
		.AddApplicationPart(GroshieHub.API.AssemblyReference.Assembly)
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