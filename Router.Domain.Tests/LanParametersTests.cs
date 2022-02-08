using System;
using System.Net;
using Router.Domain.RouterProperties;
using Xunit;

namespace Router.Domain.Tests;

public class LanParametersTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateNewInstance()
    {
        // Arrange
        var record = Record.Exception( () => new LanParameters(new MacAddress(), IPAddress.Any, new SubnetMask(0)) );
        
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
        Assert.Throws<ArgumentNullException>(() => new LanParameters(new MacAddress(), null, new SubnetMask(0)));
    }
    
    [Fact]
    public void Constructor_WithNullAsSubnetMask_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new LanParameters(new MacAddress(), IPAddress.Any, null));
    }
}