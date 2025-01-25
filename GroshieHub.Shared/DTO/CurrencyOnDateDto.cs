namespace GroshieHub.Shared.DTO;

public sealed record CurrencyOnDateDto(string Date, string Code, decimal Rate) : CurrencyDto(Code, Rate);