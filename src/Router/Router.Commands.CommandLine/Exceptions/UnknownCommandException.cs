namespace Router.Commands.CommandLine.Exceptions;

public class UnknownCommandException : CommandLineException
{
    public string Unknown { get; }

    public UnknownCommandException(string unknown, string[]? commands = null, string? message = null) 
        : base(commands ?? Array.Empty<string>(), message)
    {
        Unknown = unknown;
    }
}