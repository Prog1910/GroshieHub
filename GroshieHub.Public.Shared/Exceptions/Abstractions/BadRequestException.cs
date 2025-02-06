namespace GroshieHub.Public.Shared.Exceptions.Abstractions;

public abstract class BadRequestException(string message) : Exception(message);