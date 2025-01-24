using System.Text.Json.Serialization;
using GroshieHub.Presentation;
using GroshieHub.Presentation.Extensions;
using GroshieHub.Presentation.Formatters;

var builder = WebApplication.CreateBuilder(args);

{
	builder.SetupSerilog();

	builder.Services.AddPresentation(builder.Configuration);

	builder
		.Services.AddControllers(config =>
		{
			config.InputFormatters.Insert(0, JsonPatchFormatter.GetJsonPatchInputFormatter());
		})
		.AddJsonOptions(options =>
			options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
		);
}

var app = builder.Build();

{
	app.UseMiddleware<RequestLoggingMiddleware>();

	if (app.Environment.IsDevelopment())
		app.SetupSwagger();
	else
		app.SetupReDoc();

	app.UseCors("CorsPolicy");

	app.UseHttpsRedirection();
	app.MapControllers();
	app.Run();
}