using System;
using System.Collections.Generic;
using TpLinkConsole.CommandLine;
using Xunit;

namespace TpLinkConsole.CommandLineArguments.Tests;

public class CommandLineArgumentsParserTests
{
    private static CommandLineArgumentsParser GetParser(IReadOnlyCollection<CommandLineParameter> parameters) => new(parameters);
    private static IReadOnlyCollection<CommandLineParameter> DefaultParameters => new[]
                                                                                  {
                                                                                      new CommandLineParameter("HostAddress", "--address", "-A"),
                                                                                      new CommandLineParameter("Username", "--username", "-U"),
                                                                                      new CommandLineParameter("Password", "--password", "-P")
                                                                                  };

    private static ICommandLineArguments Parse(string[] args) => GetParser(DefaultParameters).Parse(args);

    [Fact]
    public void Parse_WithEmptyArray_ShouldReturnEmptyICommandLineArgumnets()
    {
        var actual = Parse(Array.Empty<string>());
        Assert.Empty(actual.Arguments);
        Assert.Empty(actual.MainCommand);
    }
}