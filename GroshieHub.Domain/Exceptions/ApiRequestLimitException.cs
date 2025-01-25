using GroshieHub.Domain.Exceptions.Abstractions;

namespace GroshieHub.Domain.Exceptions;

public sealed class ApiRequestLimitException()
	: BadRequestException("You have reached the request limit.");