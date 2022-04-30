namespace Router.Commands.Exceptions;

public class CommandParsingException : Exception
{
    public string[] Commands { get; }

    public CommandParsingException(string[] commands, string? message = null)
        : base(message)
    {
        Commands = commands;
    }
}