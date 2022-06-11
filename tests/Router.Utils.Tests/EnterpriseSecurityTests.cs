using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Router.Utils.Security;
using Xunit;

namespace Router.Utils.Tests;

public class EnterpriseSecurityTests
{
    public const string DefaultPassword = "p@ssw0rd";

    private static RadiusServer DefaultServer => new(DefaultPassword, IPAddress.Any);

    [Fact(DisplayName = "Constructor; With valid values; Should create instance")]
    public void Constructor_WithValidValues_ShouldCreateInstance()
    {
        new EnterpriseSecurity(DefaultServer, SecurityVersion.Automatic, EncryptionType.Auto);
        Assert.True(true);
    }

    [Fact(DisplayName = "Constructor; With valid 0 group key update period; Should create instance")]
    public void Constructor_WithZeroGroupKeyUpdatePeriod_ShouldCreateInstance()
    {
        new EnterpriseSecurity(DefaultServer, SecurityVersion.Automatic, EncryptionType.Auto, 0);
        Assert.True(true);
    }

    public static IEnumerable<object[]> ValidGroupKeyUpdatePeriod =>
        Enumerable.Range(30, 60)
                  .Select(i => new object[] {i});

    [Theory(DisplayName = "Constructor; With valid group key update period; Should create instance")]
    [MemberData(nameof(ValidGroupKeyUpdatePeriod))]
    public void Create_ValidGroupKeyUpdatePeriod(int period)
    {
        new EnterpriseSecurity(DefaultServer, SecurityVersion.Automatic, EncryptionType.Auto, period);
        Assert.True(true);
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
        Assert.ThrowsAny<Exception>(() => new EnterpriseSecurity(DefaultServer, SecurityVersion.Automatic,
                                                                 EncryptionType.Auto, groupKey));
    }

    [Fact(DisplayName = "Constructor; With null RadiusServer; Should throw exception")]
    public void Constructor_WithPasswordNull_ShouldThrowException()
    {
        Assert.ThrowsAny<Exception>(() => new EnterpriseSecurity(null!, SecurityVersion.Automatic,
                                                                 EncryptionType.Auto));
    }
}