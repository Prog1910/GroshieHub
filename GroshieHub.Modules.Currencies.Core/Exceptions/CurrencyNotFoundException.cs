using GroshieHub.Shared.Exceptions.Abstractions;

namespace GroshieHub.Modules.Currencies.Core.Exceptions;

public sealed class CurrencyNotFoundException(string code)
	: NotFoundException($"Currency with code '{code}' doesn't exist.");