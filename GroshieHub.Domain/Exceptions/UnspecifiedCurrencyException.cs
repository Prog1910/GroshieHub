using GroshieHub.Domain.Exceptions.Abstractions;

namespace GroshieHub.Domain.Exceptions;

public sealed class UnspecifiedCurrencyException()
	: InvalidRequestException("Select a currency from the list of available currencies.");