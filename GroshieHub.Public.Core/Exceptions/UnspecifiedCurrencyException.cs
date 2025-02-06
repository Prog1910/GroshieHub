using GroshieHub.Public.Shared.Exceptions.Abstractions;

namespace GroshieHub.Public.Core.Exceptions;

public sealed class UnspecifiedCurrencyException()
	: InvalidRequestException("Select a currency from the list of available currencies.");