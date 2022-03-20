namespace TpLinkConsole.CommandLine.Exceptions;

public class ArgumentValueExpectedException : CommandLineParsingException
{
    public string ParameterName { get; set; }

    public ArgumentValueExpectedException(string parameterName)
    {
        ParameterName = parameterName;
    }
    public ArgumentValueExpectedException(string parameterName, string message) 
    : base(message)
    {
        ParameterName = parameterName;
    }
}