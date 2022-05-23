using System.Collections.Generic;
using Router.Domain;
using Router.TpLink.Tests.Mocks;
using Xunit;

namespace Router.TpLink.Tests;

public class RouterCommandContextTests
{
    private RouterCommandContext Create(string[] command,
                                        RouterParameters routerParameters,
                                        Dictionary<string, string>? arguments = null) =>
        new(new FakeTpLinkRouter(routerParameters), command,
            arguments ?? new Dictionary<string, string>(), new FakeOutputFormatter());
    private RouterCommandContext Create(string[] command,
                                        Dictionary<string, string>? arguments = null) =>
        new(new FakeTpLinkRouter(RouterParameters.Default), command,
            arguments ?? new Dictionary<string, string>(), new FakeOutputFormatter());

    public static IEnumerable<object[]> SingleCommands => new[]
                                                          {
                                                              new object[] { "refresh" },
                                                              new object[] {"wlan"},
                                                              new object[] {"health"},
                                                              new object[] {"1234234"},
                                                              new object[] {"help"},
                                                              
                                                          };

    [Theory(DisplayName = "CurrentCommand; With single command; Should return exact command")]
    [MemberData(nameof(SingleCommands))]
    public void CurrentCommand_WithSingleCommand(string command)
    {
        var actual = Create(new[] {command});
        Assert.Equal(command, actual.CurrentCommand);
    }


    [Theory(DisplayName = "NextCommand; With single command; Should return null;")]
    [MemberData(nameof(SingleCommands))]
    public void NextCommand_WithSingleCommand(string command)
    {
        var actual = Create(new[] {command});
        Assert.Null(actual.NextCommand);
    }
    
    [Theory(DisplayName = "HasNextCommand; With single command; Should return false;")]
    [MemberData(nameof(SingleCommands))]
    public void HasNextCommand_WithSingleCommand(string command)
    {
        var actual = Create(new[] {command});
        Assert.False(actual.HasNextCommand);
    }

    public static IEnumerable<object[]> MultipleCommands => new[]
                                                            {
                                                                new object[] {new[] {"wlan", "status"}},
                                                                new object[] {new[] {"status", "get"}},
                                                                new object[] {new[] {"what", "happened"}},
                                                                new object[] {new[] {"this", "is", "must", "be", "truth"}},
                                                                new object[] {new[] {"wlan", "password", "12345678"}},
                                                                new object[] {new[] {"lan", "address", "192.168.0.100"}},
                                                                new object[] {new[] {"a", "b", "c", "d", "e", "f", "g"}},
                                                            };

    [Theory(DisplayName = "CurrentCommand; With multiple commands; Should point to first command after creating")]
    [MemberData(nameof(MultipleCommands))]
    public void CurrentCommand_WithMultipleCommands(string[] command)
    {
        var actual = Create(command);
        Assert.Equal(command[0], actual.CurrentCommand);
    }
    
    [Theory(DisplayName = "NextCommand; With multiple commands; Should point to second command after creating")]
    [MemberData(nameof(MultipleCommands))]
    public void NextCommand_WithMultipleCommands(string[] command)
    {
        var actual = Create(command);
        Assert.Equal(command[1], actual.NextCommand);
    }
    
    [Theory(DisplayName = "HasNextCommand; With multiple commands; Should return true after creating")]
    [MemberData(nameof(MultipleCommands))]
    public void HasNextCommand_WithMultipleCommands(string[] command)
    {
        var actual = Create(command);
        Assert.True(actual.HasNextCommand);
    }
    
    [Theory(DisplayName = "MoveNext; With multiple commands; Should switch CurrentCommand to second word after creating")]
    [MemberData(nameof(MultipleCommands))]
    public void MoveNext_WithMultipleCommands(string[] command)
    {
        var actual = Create(command);
        actual.MoveNext();
        Assert.Equal(command[1], actual.CurrentCommand);
    }

    [Theory(DisplayName = "MoveNext; With multiple commands; Should return true on non last command")]
    [MemberData(nameof(MultipleCommands))]
    public void MoveNext_WithMultipleCommands_ReturnTrueOnNonLast(string[] command)
    {
        var actual = Create(command);
        for (int i = 0; i < command.Length - 1; i++)
        {
            Assert.True(actual.MoveNext());
        }
    }

    [Theory(DisplayName = "MoveNext; With multiple commands; Should return false on last command")]
    [MemberData(nameof(MultipleCommands))]
    public void MoveNext_WithMultipleCommands_ShouldReturnFalse(string[] command)
    {
        var actual = Create(command);
        for (int i = 0; i < command.Length - 1; i++)
        {
            actual.MoveNext();
        }
        Assert.False(actual.MoveNext());
    }
}