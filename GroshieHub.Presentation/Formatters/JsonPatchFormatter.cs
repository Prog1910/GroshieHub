using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace GroshieHub.Presentation.Formatters;

public static class JsonPatchFormatter
{
	public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
		new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
			.Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>()
			.Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();
}