using GroshieHub.Shared.Enums;

namespace GroshieHub.Domain.Extensions;

public static class CurrencyCodeExtensions
{
	public static bool Exists(this string rawCode)
		=> Enum.TryParse(rawCode.ToUpperInvariant(), out CurrencyCode code) && Enum.IsDefined(code);
}