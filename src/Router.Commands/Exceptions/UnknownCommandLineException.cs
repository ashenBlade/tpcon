namespace Router.Commands.Exceptions;

public class UnknownCommandLineException : CommandLineParsingException
{
    public UnknownCommandLineException(string[]? commands = null, string? message = null) 
        : base(commands ?? Array.Empty<string>(), message) 
    { }
}