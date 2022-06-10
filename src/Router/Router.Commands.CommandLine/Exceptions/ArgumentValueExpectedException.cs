namespace Router.Commands.CommandLine.Exceptions;

public class ArgumentValueExpectedException : CommandLineException
{
    public string Expected { get; }

    public ArgumentValueExpectedException(string expected, IEnumerable<string> commands, string? message = null) :
        base(commands, message)
    {
        Expected = expected;
    }
}