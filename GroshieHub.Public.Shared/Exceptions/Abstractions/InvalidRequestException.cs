namespace GroshieHub.Public.Shared.Exceptions.Abstractions;

public abstract class InvalidRequestException(string message) : Exception(message);