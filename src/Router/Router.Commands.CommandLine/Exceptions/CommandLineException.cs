namespace Router.Commands.CommandLine.Exceptions;

public class CommandLineException : Exception
{
    public string[] Commands { get; }

    public CommandLineException(IEnumerable<string> commands, string? message = null)
        : base(message)
    {
        Commands = commands.ToArray();
    }
}