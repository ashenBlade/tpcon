namespace Router.Commands.CommandLine.Exceptions;

public class DuplicatedArgumentsException : CommandLineException
{
    public string Duplicated { get; }

    public DuplicatedArgumentsException(string duplicated, string[] commands, string? message = null) : base(commands, message)
    {
        Duplicated = duplicated;
    }
}