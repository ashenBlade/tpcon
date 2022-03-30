using System.Runtime.CompilerServices;
using TpLinkConsole.CommandLine;
using TpLinkConsole.CommandLine.Exceptions;

var parser = new CommandLineArgumentsParser();
try
{
    var cmd = parser.Parse(args);
}
catch (ArgumentValueExpectedException argumentValueExpectedException)
{
    Console.WriteLine($"Value expected for parameter: {argumentValueExpectedException.ParameterName}");
    return;
}
catch (ArgumentExpectedException argumentExpectedException)
{
    Console.WriteLine($"Argument expected");
    return;
}
catch (CommandLineParsingException cmdParsingException)
{
    Console.WriteLine("Invalid arguments");
    return;
}
