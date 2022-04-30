namespace Router.Commands.Exceptions;

public class UnknownCommandException : CommandParsingException
{
    public UnknownCommandException(string[]? commands = null, string? message = null) : base(commands ?? Array.Empty<string>(), message) 
    { }
}