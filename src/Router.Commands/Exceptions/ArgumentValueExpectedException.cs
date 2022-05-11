namespace Router.Commands.Exceptions;

public class ArgumentValueExpectedException: CommandLineParsingException
{
    public string Expected { get; }

    public ArgumentValueExpectedException(string expected, string[] commands, string? message = null) : base(commands, message)
    {
        Expected = expected;
    }
}