namespace Router.Commands.Exceptions;

public class DuplicatedArgumentsException : CommandLineParsingException
{
    public string Duplicated { get; }

    public DuplicatedArgumentsException(string duplicated, string[] commands, string? message = null) : base(commands, message)
    {
        Duplicated = duplicated;
    }
}