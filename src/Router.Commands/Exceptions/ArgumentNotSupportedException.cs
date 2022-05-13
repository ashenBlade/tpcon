namespace Router.Commands.Exceptions;

public class ArgumentNotSupportedException : CommandLineException
{
    public ArgumentNotSupportedException(string[] commands, string? message = null) : base(commands, message)
    { }
}