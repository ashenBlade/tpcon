using System;
using System.Net;
using Router.Domain.Lan;
using Xunit;

namespace Router.Domain.Tests;

public class LanParametersTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateNewInstance()
    {
        var record = Record.Exception( () => new LanParameters(MacAddress.Parse("11-11-11-11-11-11"), IPAddress.Any, new SubnetMask(0)) );
        
        Assert.Null(record);
    }

    [Fact]
    public void Constructor_WithNullAsMacAddress_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new LanParameters(null, IPAddress.Any, new SubnetMask(0)));
    }
    
    [Fact]
    public void Constructor_WithNullAsIpAddress_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new LanParameters(MacAddress.Parse("11-11-11-11-11-11"), null, new SubnetMask(0)));
    }
}