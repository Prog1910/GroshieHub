namespace GroshieHub.Public.Shared.Extensions;

public static class DecimalExtensions
{
	public static decimal RoundToDecimalPlaces(this decimal value, int decimalPlaces)
		=> Math.Round(value, decimalPlaces, MidpointRounding.ToZero);
}