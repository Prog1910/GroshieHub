using GroshieHub.Shared.Exceptions.Abstractions;

namespace GroshieHub.Shared.Exceptions;

public sealed class ApiRequestLimitException()
	: BadRequestException("You have reached the request limit.");