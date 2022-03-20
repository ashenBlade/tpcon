using System;
using System.Collections.Generic;
using System.Linq;
using TpLinkConsole.CommandLine;
using Xunit;

namespace TpLinkConsole.CommandLineArguments.Tests;

public class CommandLineArgumentsParserTests
{
    private static CommandLineArgumentsParser GetParser() => new();

    private static ICommandLineArguments Parse(string[] args) => GetParser().Parse(args);

    [Fact]
    public void Parse_WithEmptyArray_ShouldReturnEmptyICommandLineArguments()
    {
        var actual = Parse(Array.Empty<string>());
        Assert.Empty(actual.Arguments);
        Assert.Empty(actual.MainCommand);
    }

    [Fact]
    public void Parse_WithSingleCommandSet_ShouldReturnArgumentsWithSameCommand()
    {
        // Arrange
        const string command = "wlan";
        // Act
        var actual = Parse(new[] {command});
        // Assert
        Assert.Single(actual.MainCommand);
        Assert.Same(command, actual.MainCommand.First());
    }

    public static IEnumerable<object[]> MultipleCommands => new[]
                                                            {
                                                                new object[]{new[] {"wlan", "status"}}, 
                                                                new object[]{new[] {"lan", "address"}},
                                                                new object[]{new[] {"wlan", "ssid"}},
                                                                new object[]{new[] {"hello", "world", "it", "is", "me"}}
                                                            };

    [Theory]
    [MemberData(nameof(MultipleCommands))]
    public void Parse_WithMultipleCommandWords_ShouldReturnArgumentsWithEachCommand(string[] commands)
    {
        var actual = Parse(commands);
        Assert.Equal(commands, actual.MainCommand);
    }

    public static IEnumerable<object[]> SingleArguments => new[]
                                                           {
                                                               new object[] {new[] {"--address", "192.168.0.1"}},
                                                               new object[] {new[] {"--argument", "value"}},
                                                               new object[] {new[] {"--username", "admin"}},
                                                               new object[] {new[] {"--password", "P@ssw0rd"}},
                                                               new object[] {new[] {"--output", "json"}},
                                                           };

    [Theory]
    [MemberData(nameof(SingleArguments))]
    public void Parse_WithSingleArgument_ShouldReturnSingleArgumentWithSameValue(string[] arguments)
    {
        var actual = Parse(arguments);
        var parameter = arguments[0];
        var value = arguments[1];
        var args = actual.Arguments.ToList();
        Assert.Single(args);
        Assert.Equal(args[0].Value, value);
        Assert.Equal(args[0].Name, parameter);
    }

    public static IEnumerable<object[]> MultipleArguments => new[]
                                                           {
                                                               new object[] {new[] {"--address", "192.168.0.1", "--date", "20.03.22"}},
                                                               new object[] {new[] {"--argument", "value", "--time", "19:30"}},
                                                               new object[] {new[] {"--username", "admin", "--offset", "20"}},
                                                               new object[] {new[] {"--password", "P@ssw0rd", "--value", "default", "--target", "all"}},
                                                               new object[] {new[] {"--output", "json", "--verbose", "true"}},
                                                           };
    
    [Theory]
    [MemberData(nameof(MultipleArguments))]
    public void Parse_WithMultipleArguments_ShouldReturnMultipleArgumentsWithSameValues(string[] arguments)
    {
        var commands = Parse(arguments);
        var numberOfArguments = arguments.Length / 2;
        var args = commands.Arguments.ToList();
        Assert.Equal(numberOfArguments, args.Count);
        Assert.Equal(arguments, args.SelectMany(arg => new[]{arg.Name, arg.Value}));
    }

    public static IEnumerable<object[]> ArgumentsAfterCommands => new[]
                                                           {
                                                               new object[] {new[] {"wlan", "status"}, new[] {"--username", "admin"}},
                                                               new object[] {new[] {"statistics"}, new[] {"--address", "192.168.0.100"}},
                                                               new object[] {new[] {"wlan"}, new[] {"--username", "admin"}},
                                                               new object[] {new[] {"lan", "netmask"}, new[] {"--output", "json"}},
                                                           };
    
    [Theory]
    [MemberData(nameof(ArgumentsAfterCommands))]
    public void Parse_WithArgumentsAfterCommand_ShouldReturnCommandAndArguments(string[] commands, string[] arguments)
    {
        var actual = Parse(commands.Concat(arguments).ToArray());
        var numOfArguments = arguments.Length / 2;
        var actualArguments = actual.Arguments.ToList();
        Assert.Equal(numOfArguments, actualArguments.Count);
        Assert.Equal(arguments, actualArguments.SelectMany(arg => new[]{arg.Name, arg.Value}));
        Assert.Equal(commands, actual.MainCommand);
    }
}