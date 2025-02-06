using GroshieHub.Public.Shared.Exceptions.Abstractions;

namespace GroshieHub.Public.Shared.Exceptions;

public sealed class ApiRequestLimitException()
	: BadRequestException("You have reached the request limit.");