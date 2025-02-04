using Serilog;

namespace GroshieHub.Bootstrapper.Extensions;

public sealed class RequestLoggingMiddleware(RequestDelegate next)
{
	public async Task InvokeAsync(HttpContext context)
	{
		var request = context.Request;
		Log.Information("Request - {Method} {Path} {QueryString}", request.Method, request.Path, request.QueryString);
		await next(context);
	}
}