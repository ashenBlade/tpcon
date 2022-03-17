using TpLinkConsole.CommandLine;
using Xunit;

namespace TpLinkConsole.CommandLineArguments.Tests;

public class CommandLineArgumentsParserTests
{
    private static CommandLineArgumentsParser Parser => new();
    private static ICommandLineArguments Parse(string[] args) => Parser.Parse(args);
}