using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Router.Utils.Security;
using Xunit;

namespace Router.Utils.Tests;

public class RadiusServerTests
{
    public const string DefaultPassword = "p@ssw0rd";

    [Theory(DisplayName = "Constructor; With valid password; Should create instance")]
    [InlineData("p@ssw0rd")]
    [InlineData("12345678")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData(" ")]
    [InlineData("dfdfd")]
    [InlineData("d")]
    [InlineData("sdfghjkjhgfd")]
    public void Constructor_WithValidPassword_ShouldCreateInstance(string password)
    {
        new RadiusServer(password, IPAddress.None);
    }

    [Fact(DisplayName = "Constructor; With password null or empty; Should throw exception")]
    public void Constructor_WithPasswordNullOrEmpty_ShouldThrowException()
    {
        Assert.ThrowsAny<Exception>(() => new RadiusServer(null, IPAddress.Any));
        Assert.ThrowsAny<Exception>(() => new RadiusServer(string.Empty, IPAddress.Any));
    }

    public static IEnumerable<object[]> LongPasswords =>
        new[]
        {
            new object[] {Enumerable.Repeat("a", 253).Aggregate((s, n) => $"{s}{n}"),},
            new object[] {Enumerable.Repeat("ad", 200).Aggregate((s, n) => $"{s}{n}"),},
            new object[] {Enumerable.Repeat("1", 300).Aggregate((s, n) => $"{s}{n}"),}
        };

    [Theory(DisplayName = "Constructor; With password longer than 64; Should throw exception")]
    [MemberData(nameof(LongPasswords))]
    public void Constructor_WithPasswordLong_ShouldThrowException(string @long)
    {
        Assert.ThrowsAny<Exception>(() => new RadiusServer(@long, IPAddress.Any));
    }

    [Theory(DisplayName = "Constructor; With negative port number; Should throw exception")]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-3)]
    [InlineData(int.MinValue)]
    public void Constructor_WithNegativePortNumber_ShouldThrowException(int port)
    {
        Assert.ThrowsAny<Exception>(() => new RadiusServer(DefaultPassword, IPAddress.Any, port));
    }

    [Fact(DisplayName = "Constructor; With 0 port number; Should initialize Port with default value (1812)")]
    public void Constructor_WithProvided0Port_ShouldInitializePortWithDefaultValue()
    {
        var server = new RadiusServer(DefaultPassword, IPAddress.Any, 0);
        Assert.Equal(RadiusServer.DefaultPort, server.Port);
    }
}