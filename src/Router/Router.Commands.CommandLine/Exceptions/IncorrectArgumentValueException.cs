namespace Router.Commands.CommandLine.Exceptions;

public class IncorrectArgumentValueException : CommandLineException
{
    public string Argument { get; }
    public string Actual { get; }
    public string? Expected { get; }

    public IncorrectArgumentValueException(string argument,
                                           string actual,
                                           string? expected,
                                           IEnumerable<string> commands,
                                           string? message = null)
        : base(commands, message)
    {
        Argument = argument;
        Actual = actual;
        Expected = expected;
    }

    public IncorrectArgumentValueException(string argument,
                                           string actual,
                                           IEnumerable<string> commands,
                                           string? message = null)
        : this(argument, actual, null, commands, message)
    {
    }
}