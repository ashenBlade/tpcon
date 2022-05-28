namespace Router.Commands.Exceptions;

public class CommandLineException : Exception
{
    public string[] Commands { get; }

    public CommandLineException(string[] commands, string? message = null)
        : base(message)
    {
        Commands = commands;
    }
}