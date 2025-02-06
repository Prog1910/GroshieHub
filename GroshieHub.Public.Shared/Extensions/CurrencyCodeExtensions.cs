using GroshieHub.Public.Shared.Enums;

namespace GroshieHub.Public.Shared.Extensions;

public static class CurrencyCodeExtensions
{
	public static bool Exists(this string rawCode)
		=> Enum.TryParse(rawCode.ToUpperInvariant(), out CurrencyCode code) && Enum.IsDefined(code);
}