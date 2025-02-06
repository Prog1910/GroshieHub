namespace GroshieHub.Public.Shared.Extensions;

public static class DateTimeExtensions
{
	public static string FormatDateTime(this DateTime date, string format)
		=> date.ToString(format);
}