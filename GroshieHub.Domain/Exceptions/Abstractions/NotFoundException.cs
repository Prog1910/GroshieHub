namespace GroshieHub.Domain.Exceptions.Abstractions;

public abstract class NotFoundException(string message) : Exception(message);