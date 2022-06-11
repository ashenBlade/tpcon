using System;
using System.Collections.Generic;
using System.Linq;
using Router.Utils.Security;
using Xunit;

namespace Router.Utils.Tests;

public class PersonalSecurityTests
{
    public const string DefaultPassword = "p@ssw0rd";

    public static IEnumerable<object[]> ValidPasswords =>
        new[]
        {
            new[] {"valid_password"}, new[] {"12345678"}, new[] {"_p@ssw0rd_"}, new[] {"a1s2d3f4"},
            new[] {"qwertyuiop"},
        };

    [Theory(DisplayName = "Constructor; With valid password; Should create instance")]
    [MemberData(nameof(ValidPasswords))]
    public void Create_ValidPassword(string password)
    {
        new PersonalSecurity(password, EncryptionType.Auto, SecurityVersion.Automatic);
        Assert.True(true);
    }

    [Fact(DisplayName = "Constructor; With valid 0 group key update period; Should create instance")]
    public void Constructor_WithZeroGroupKeyUpdatePeriod_ShouldCreateInstance()
    {
        new PersonalSecurity(DefaultPassword, EncryptionType.Auto, SecurityVersion.Automatic, 0);
        Assert.True(true);
    }

    public static IEnumerable<object[]> ValidGroupKeyUpdatePeriod =>
        Enumerable.Range(30, 60)
                  .Select(i => new object[] {i});

    [Theory(DisplayName = "Constructor; With valid group key update period; Should create instance")]
    [MemberData(nameof(ValidGroupKeyUpdatePeriod))]
    public void Create_ValidGroupKeyUpdatePeriod(int period)
    {
        new PersonalSecurity(DefaultPassword, EncryptionType.Auto, SecurityVersion.Automatic, period);
        Assert.True(true);
    }

    [Theory(DisplayName = "Constructor; With password shorter than 8; Should throw exception")]
    [InlineData("short")]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("1234567")]
    [InlineData("       ")]
    [InlineData("pasword")]
    public void Constructor_WithPasswordShorterThan8_ShouldThrowException(string @short)
    {
        Assert.ThrowsAny<Exception>(() => new PersonalSecurity(@short, EncryptionType.Auto, SecurityVersion.Automatic));
    }

    [Theory(DisplayName = "Constructor; With password longer than 64; Should throw exception")]
    [InlineData("1234567890_1234567890_1234567890_1234567890_1234567890_01234567890_1234567890")]
    [InlineData("some_very_long_password_that_should_cause_exception_throwing_in_constructor")]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345")]
    public void Constructor_WithPasswordLongerThan63_ShouldThrowException(string @long)
    {
        Assert.ThrowsAny<Exception>(() => new PersonalSecurity(@long, EncryptionType.Auto, SecurityVersion.Automatic));
    }

    [Theory(DisplayName = "Constructor; With invalid group key update period; Should throw exception")]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(29)]
    [InlineData(10)]
    [InlineData(-10)]
    [InlineData(int.MinValue)]
    [InlineData(-2)]
    public void Constructor_WithInvalidGroupKeyUpdatePeriod_ShouldThrowException(int groupKey)
    {
        Assert.ThrowsAny<Exception>(() => new PersonalSecurity(DefaultPassword, EncryptionType.Auto,
                                                               SecurityVersion.Automatic, groupKey));
    }

    [Fact(DisplayName = "Constructor; With null is password; Should throw exception")]
    public void Constructor_WithPasswordNull_ShouldThrowException()
    {
        Assert.ThrowsAny<Exception>(() => new PersonalSecurity(null, EncryptionType.Auto, SecurityVersion.Automatic));
    }
}