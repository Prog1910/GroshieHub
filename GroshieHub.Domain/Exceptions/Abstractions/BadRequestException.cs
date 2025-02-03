namespace GroshieHub.Domain.Exceptions.Abstractions;

public abstract class BadRequestException(string message) : Exception(message);