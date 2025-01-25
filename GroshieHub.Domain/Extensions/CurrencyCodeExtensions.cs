using GroshieHub.Domain.Entities;

namespace GroshieHub.Domain.Extensions;

public static class CurrencyCodeExtensions
{
	public static bool IsInvalid(this ECurrencyCode code)
		=> code == ECurrencyCode.UNKNOWN || !Enum.IsDefined(code);
}