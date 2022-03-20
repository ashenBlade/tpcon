namespace TpLinkConsole.CommandLine.Exceptions;

public class CommandLineParsingException : Exception
{
    public CommandLineParsingException()
    { }
    public CommandLineParsingException(string message) 
        : base(message)
    { }
}