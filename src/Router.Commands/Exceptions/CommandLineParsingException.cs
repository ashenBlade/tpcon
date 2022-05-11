namespace Router.Commands.Exceptions;

public class CommandLineParsingException : Exception
{
    public string[] Commands { get; }

    public CommandLineParsingException(string[] commands, string? message = null)
        : base(message)
    {
        Commands = commands;
    }
}