using GroshieHub.Domain.Exceptions.Abstractions;

namespace GroshieHub.Domain.Exceptions;

public sealed class CurrencyNotFoundException(string code)
	: NotFoundException($"Currency with code '{code}' doesn't exist.");