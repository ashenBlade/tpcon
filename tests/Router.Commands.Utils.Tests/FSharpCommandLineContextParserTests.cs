using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Router.Commands.Exceptions;
using Router.Domain;
using Xunit;

namespace Router.Commands.Utils.Tests;

public class FSharpCommandLineContextParserTests
{
    private static ICommandLineContextParser CreateParser() => 
        new FSharpCommandLineParser();
    private static CommandLineContext Parse(params string[] args) => 
        CreateParser()
           .ParseCommandLineContext(args);
    
    [Fact(DisplayName = "With: empty input; Return: context with empty command array and default router parameters")]
    public void EmptyInput()
    {
        var actual = Parse(Array.Empty<string>());
        Assert.Empty(actual.Command);
        Assert.Equal(RouterParameters.Default, actual.RouterParameters);
    }
    
    [Fact(DisplayName = "With: empty input; Should: return false on HasNextCommand")]
    public void EmptyInput_CheckHasNext()
    {
        var actual = Parse(Array.Empty<string>());
        Assert.False(actual.HasNextCommand);   
    }

    public static IEnumerable<object[]> SingleCommandsWithoutOptions => new[]
                                                                        {
                                                                            new object[] {"refresh"},
                                                                            new object[] {"check"},
                                                                            new object[] {"1234534"},
                                                                            new object[] {"some-command"},
                                                                            new object[] {"wlan"},
                                                                        };

    [Theory(DisplayName = "With: single command; Should: return context with single command array and default router parameters")]
    [MemberData(nameof(SingleCommandsWithoutOptions))]
    public void SingleCommand(string command)
    {
        var actual = Parse(command);
        Assert.Single(actual.Command);
        Assert.Equal(command, actual.Command[0]);
    }

    [Theory(DisplayName = "With: single command; Should: return first command as in input on CurrentCommand")]
    [MemberData(nameof(SingleCommandsWithoutOptions))]
    public void SingleCommand_CheckCurrentCommand(string command)
    {
        var actual = Parse(command);
        Assert.Equal(command, actual.CurrentCommand);
    }

    public static IEnumerable<object[]> MultipleCommandsWithoutOptions => new[]
                                                                          {
                                                                              new object[]
                                                                              {
                                                                                  new[]{"wlan", "status"}
                                                                              },
                                                                              new object[] 
                                                                              {
                                                                                  new[]{ "wlan", "password", "2345765" } 
                                                                              },
                                                                              new object[]
                                                                              {
                                                                                  new[]{"status", "all"}
                                                                              },
                                                                              new object[]
                                                                              {
                                                                                  new[]{"set", "defence", "true"}
                                                                              },
                                                                              new object[]
                                                                              {
                                                                                  new[]{"what", "is", "happening"}
                                                                              },
                                                                              new object[] 
                                                                              {
                                                                                  new[]{ "simple", "command", "just", "for", "testing" }
                                                                              },
                                                                          };

    [Theory(DisplayName = "With: multiple commands; Should: return context with same commands sequence as in Command")]
    [MemberData(nameof(MultipleCommandsWithoutOptions))]
    public void MultipleCommands(string[] commands)
    {
        var actual = Parse(commands);
        Assert.Equal(commands, actual.Command);
    }

    public static IEnumerable<object[]> OnlyRouterParameters => new[]
                                                                {
                                                                    new object[] { new[]{"--username", "admin"}, },
                                                                    new object[] { new[]{"--username", "admin", "--password", "12345678"}, },
                                                                    new object[] { new[]{"--address", "192.168.0.1"}, },
                                                                    new object[] { new[]{"--address", "192.168.0.1", "--username", "admin", "--password", "admin"}, },
                                                                    new object[] { new[]{"--address", "192.168.0.1"}, },
                                                                };

    [Theory(DisplayName = "With: only arguments; Should: return empty command array")]
    [MemberData(nameof(OnlyRouterParameters))]
    public void NoCommand_WithArguments(string[] args)
    {
        var actual = Parse(args);
        Assert.Empty(actual.Command);
    }

    private static string[] GetArgs(string? username, string? password, string? address)
    {
        var list = new List<string>();
        if (username is not null)
        {
            list.Add("--username");
            list.Add(username);
        }
        
        if (password is not null)
        {
            list.Add("--password");
            list.Add(password);
        }
        
        if (address is not null)
        {
            list.Add("--address");
            list.Add(address);
        }
        
        return list.ToArray();
    }

    private static Dictionary<string, string> ArgsToDict(string[] args)
    {
        var dict = new Dictionary<string, string>();
        for (int i = 0; i < args.Length; i++)
        {
            if (!args[i].StartsWith('-'))
            {
                continue;
            }

            var arg = args[i][2..];
            var value = args[i + 1];
            dict[arg] = value;
            i++;
        }

        return dict;
    }

    private static RouterParameters GetRouterParametersFromArgs(string[] args)
    {
        var dict = ArgsToDict(args);
        dict.TryGetValue("username", out var username);
        dict.TryGetValue("address", out var address);
        dict.TryGetValue("password", out var password);
        return new RouterParameters(address is null ? null : IPAddress.Parse(address), username, password);
    }

    public static IEnumerable<object?[]> UsernamePasswordAddressSequences => new[]
                                                                             {
                                                                                 new object?[]
                                                                                 {
                                                                                     "admin", "admin", "192.168.0.1"
                                                                                 },
                                                                                 new object?[]
                                                                                 {
                                                                                     "total", "control","12.65.55.103"
                                                                                 },
                                                                                 new object?[]
                                                                                 {
                                                                                     "some-name", "some-password", "221.2.11.103"
                                                                                 },
                                                                                 new object?[]
                                                                                 {
                                                                                     "username", "P@ssw0rd", "123.90.37.22"
                                                                                 },
                                                                                 new object?[]
                                                                                 {
                                                                                     "username", "password", "192.168.0.1",
                                                                                 },
                                                                                 new object?[]
                                                                                 {
                                                                                     "admin", "password", "192.168.0.1",
                                                                                 },
                                                                                 new object?[]
                                                                                 {
                                                                                     "username", "admin", "192.168.0.1",
                                                                                 },
                                                                                 new object?[]
                                                                                 {
                                                                                     "admin", "admin", "222.222.222.222",
                                                                                 },
                                                                             };

    [Theory(DisplayName = "With: only arguments; Should: contain every router parameter provided")]
    [MemberData(nameof(UsernamePasswordAddressSequences))]
    public void NoCommands_WithOnlyArguments(string? username, string? password, string? address)
    {
        var args = GetArgs(username, password, address);
        var expected = GetRouterParametersFromArgs(args);
        
        var actual = Parse(args);
        
        Assert.Equal(expected, actual.RouterParameters);
    }

    public static IEnumerable<object[]> CommandsAndRouterParameters => new[] 
                                                                       {
                                                                           new object[]
                                                                           {
                                                                                new[]{"refresh"},
                                                                                new[]{"commander", "p2pconnect", "192.168.0.101"}
                                                                           },
                                                                           new object[]
                                                                           {
                                                                               new[]{"health"},
                                                                               new[]{"admin", "admin", "192.168.0.1"}
                                                                           },
                                                                           new object[]
                                                                           {
                                                                               new[]{"wlan", "status"},
                                                                               new[]{"some-name", "password", "19.34.22.1"}
                                                                           },
                                                                           new object[]
                                                                           {
                                                                               new[]{"lan", "address", "192.168.0.101"},
                                                                               new[]{"admin", "admin", "23.55.90.2"}
                                                                           },
                                                                           new object[]
                                                                           {
                                                                               new[]{"refresh"},
                                                                               new[]{"1234567", "admin", "90.0.3.22"}
                                                                           },
                                                                       };

    [Theory(DisplayName = "With: commands and router parameters arguments; Should: return same commands and router parameters")]
    [MemberData(nameof(CommandsAndRouterParameters))]
    public void CommandsAndArguments(string[] commandsRaw, string[] parameters)
    {
        var routerParametersArgsRaw = GetArgs(parameters[0], parameters[1], parameters[2]);
        var routerParameters = GetRouterParametersFromArgs(routerParametersArgsRaw);
        var strings = commandsRaw.Concat(routerParametersArgsRaw).ToArray();
        var actual = Parse(strings);
        
        Assert.Equal(routerParameters, actual.RouterParameters);
        Assert.Equal(commandsRaw, actual.Command);
    }

    public static IEnumerable<object[]> OnlyArguments => new[]
                                                         {
                                                             new object[] {new[] {"argument", "value"}},
                                                             new object[] {new[] {"arg", "val"}},
                                                             new object[] {new[] {"output", "json", "what", "no", "argument", "value"}},
                                                             new object[] {new[] {"type", "day", "verbose", "false"}},
                                                             new object[] {new[] {"input", "date", "extra", "name"}},
                                                             new object[] {new[] {"argument", "value"}},
                                                             new object[] {new[] {"username", "admin"}},
                                                             new object[] {new[] {"password", "admin", "address", "192.168.0.1"}},
                                                         };

    private static string[] DecorateArgumentsWithDashes(string[] args)
    {
        var result = new string[args.Length];
        for (var i = 0; i < args.Length; i+=2)
        {
            result[i] = $"--{args[i]}";
            result[i + 1] = args[i + 1];
        }
        return result;
    }
    
    [Theory(DisplayName = "With: only arguments; Should: return same arguments in Arguments")]
    [MemberData(nameof(OnlyArguments))]
    public void OnlyArguments_ReturnSameArguments(string[] args)
    {
        var actual = Parse(DecorateArgumentsWithDashes(args));
        for (int i = 0; i < args.Length; i += 2)
        {
            Assert.Equal(args[i + 1], actual.Arguments[args[i]]);
        }
    }

    public static IEnumerable<object[]> ArgumentsWithoutProvidedValues => new[]
                                                                          {
                                                                              new object[] {new[] {"--username"},},
                                                                              new object[]
                                                                              {
                                                                                  new[]
                                                                                  {
                                                                                      "--username", "admin",
                                                                                      "--password"
                                                                                  },
                                                                              },
                                                                              new object[]
                                                                              {
                                                                                  new[]
                                                                                  {
                                                                                      "--address", "192.168.0.1",
                                                                                      "--output"
                                                                                  },
                                                                              },
                                                                              new object[] {new[] {"--argument"},},
                                                                          };

    [Theory]
    [MemberData(nameof(ArgumentsWithoutProvidedValues))]
    public void OnlyArguments_WithoutProvidedValue_ThrowArgumentValueExpectedException(string[] args)
    {
        Assert.Throws<ArgumentValueExpectedException>(() => Parse(args));
    }
}