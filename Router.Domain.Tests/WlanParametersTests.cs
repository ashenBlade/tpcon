using System;
using Router.Domain.RouterProperties;
using Xunit;

namespace Router.Domain.Tests;

public class WlanParametersTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateNewInstance()
    {
        var parameters = new WlanParameters("Hello, World", "Password", true);

        Assert.True(true);
    }

    [Fact]
    public void Constructor_WithNullInPasswordParameter_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WlanParameters("Hello, world", null, true));
    }

    [Fact]
    public void Constructor_WithNullInSSIDParameter_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WlanParameters(null, "Password", true));
    }

    [Fact]
    public void WithPassword_WithNewPassword_ShouldReturnParameterWithNewPassword()
    {
        const string OldPassword = "OLD_PASSWORD";
        const string NewPassword = "NEW_PASSWORD";
        // Arrange
        var old = new WlanParameters("Hello, world", OldPassword, true);
        // Act
        var newParams = old.WithPassword(NewPassword);
        // Assert
        Assert.NotEqual(old.Password, newParams.Password);
    }
    
    [Fact]
    public void WithSSID_WithNewSSID_ShouldReturnParameterWithNewSSID()
    {
        const string OldSSID = "OLD_SSID";
        const string NewSSID = "NEW_SSID";
        // Arrange
        var old = new WlanParameters("Hello, world", OldSSID, true);
        // Act
        var newParams = old.WithSSID(NewSSID);
        // Assert
        Assert.NotEqual(old.SSID, newParams.SSID);
    }
    
    [Fact]
    public void WithActiveState_WithNewState_ShouldReturnParameterWithNewState()
    {
        const bool OldState = false;
        const bool NewState = true;
        // Arrange
        var old = new WlanParameters("Hello, world", "Password", OldState);
        // Act
        var newParams = old.WithActiveState(NewState);
        // Assert
        Assert.NotEqual(old.IsActive, newParams.IsActive);
    }

    [Fact]
    public void WithMethods_WithAnyValues_ShouldNotModifyOldValue()
    {
        
        // Arrange
        const string OldSSID = "Hello, world";
        const string OldPassword = "Password";
        const bool OldState = true;
        
        var old = new WlanParameters(OldSSID, OldPassword, OldState);
        // Act
        var modified1 = old.WithSSID(OldSSID + " something new");
        var modified2 = old.WithPassword(OldPassword + " something new");
        var modified3 = old.WithActiveState(!OldState);
        // Assert
        Assert.NotEqual(old, modified1);
        Assert.NotEqual(old, modified2);
        Assert.NotEqual(old, modified3);
        Assert.Equal(old.Password, OldPassword);
        Assert.Equal(old.SSID, OldSSID);
        Assert.Equal(old.IsActive, OldState);
    }
}