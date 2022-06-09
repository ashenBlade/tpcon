namespace Router.Commands.CommandLine.Exceptions;

public class ArgumentNotProvidedException : CommandLineException
{
    public string Expected { get; }

    public ArgumentNotProvidedException(string expected, string[] commands, string? message = null) 
        : base(commands, message)
    {
        Expected = expected;
    }
}