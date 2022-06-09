namespace Router.Commands.CommandLine.Exceptions;

public class ArgumentNotSupportedException : CommandLineException
{
    public ArgumentNotSupportedException(string[] commands, string? message = null) : base(commands, message)
    { }
}