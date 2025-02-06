using GroshieHub.Public.Shared.Exceptions.Abstractions;

namespace GroshieHub.Public.Core.Exceptions;

public sealed class CurrencyNotFoundException(string code)
	: NotFoundException($"Currency with code '{code}' doesn't exist.");