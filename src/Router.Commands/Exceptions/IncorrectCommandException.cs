namespace Router.Commands.Exceptions;

public class IncorrectCommandException : CommandLineException
{
    public string Incorrect { get; }

    public IncorrectCommandException(string incorrect, string[] commands, string? message = null) 
        : base(commands, message)
    {
        Incorrect = incorrect;
    }
}