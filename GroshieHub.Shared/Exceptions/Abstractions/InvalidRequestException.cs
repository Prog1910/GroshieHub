namespace GroshieHub.Shared.Exceptions.Abstractions;

public abstract class InvalidRequestException(string message) : Exception(message);