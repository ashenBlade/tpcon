namespace Router.Commands.Exceptions;

public class ArgumentValueExpectedException: CommandParsingException
{
    public string Argument { get; }

    public ArgumentValueExpectedException(string argument, string[] commands, string? message = null) : base(commands, message)
    {
        Argument = argument;
    }
}