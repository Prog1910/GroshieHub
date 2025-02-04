namespace GroshieHub.Shared.Exceptions.Abstractions;

public abstract class NotFoundException(string message) : Exception(message);