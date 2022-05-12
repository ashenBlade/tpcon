namespace Router.Commands.Exceptions;

public class IncorrectArgumentValueException : CommandLineException
{
    public string Argument { get; }
    public string Actual { get; }
    public string? Expected { get; }

    public IncorrectArgumentValueException(string argument, string actual, string? expected, string[] commands, string? message = null) 
        : base(commands, message)
    {
        Argument = argument;
        Actual = actual;
        Expected = expected;
    }
    
    public IncorrectArgumentValueException(string argument, string actual, string[] commands, string? message = null)
        : this(argument, actual, null, commands, message) 
    { }
}