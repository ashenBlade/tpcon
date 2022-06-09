namespace Router.Commands.CommandLine.Exceptions;

public class IncompleteCommandException : CommandLineException
{
    public IncompleteCommandException(string[] commands, string? message = null) : base(commands, message)
    { }
}