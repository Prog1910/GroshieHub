using GroshieHub.Shared.Exceptions.Abstractions;

namespace GroshieHub.Modules.Currencies.Core.Exceptions;

public sealed class UnspecifiedCurrencyException()
	: InvalidRequestException("Select a currency from the list of available currencies.");