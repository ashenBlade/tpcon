using System.Collections.Generic;
using TpLinkConsole.CommandLine;
using Xunit;

namespace TpLinkConsole.CommandLineArguments.Tests;

public class CommandLineArgumentsParserTests
{
    private static CommandLineArgumentsParser GetParser(IReadOnlyCollection<CommandLineParameter> parameters) => new(parameters);
    private static IReadOnlyCollection<CommandLineParameter> DefaultParameters => new[] { new CommandLineParameter("") };

    private static ICommandLineArguments Parse(string[] args) => GetParser(DefaultParameters).Parse(args);
}