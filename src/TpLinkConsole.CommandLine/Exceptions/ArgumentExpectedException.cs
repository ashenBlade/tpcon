namespace TpLinkConsole.CommandLine.Exceptions;

public class ArgumentExpectedException : CommandLineParsingException
{
    public ArgumentExpectedException()
    { }
    public ArgumentExpectedException(string message) 
        : base(message) 
    { }
}