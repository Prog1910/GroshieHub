namespace GroshieHub.Domain.Exceptions.Abstractions;

public abstract class InvalidRequestException(string? message) : Exception(message);