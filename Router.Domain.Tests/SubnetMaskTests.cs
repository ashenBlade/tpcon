using System;
using System.Net;
using Xunit;
using Xunit.Sdk;

namespace Router.Domain.Tests;

public class SubnetMaskTests
{
    [Fact]
    public void Constructor_WithSubnetMaskLength8_ShouldHaveLength8AfterCreating()
    {
        var length = 8;
        // Arrange
        var mask = new SubnetMask(length);

        // Assert
        Assert.Equal(length, mask.MaskLength);
    }

    [Fact]
    public void Constructor_WithLengthMinus7_ShouldThrowArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SubnetMask(-7));
    }

    [Fact]
    public void Constructor_WithNegativeMaskLength_ShouldThrowArgumentOutOfRangeException()
    {
        var length = Random.Shared.Next(int.MinValue, -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => new SubnetMask(length));
    }

    [Fact]
    public void Constructor_WithLength33_ShouldThrowArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SubnetMask(33));
    }

    [Fact]
    public void Constructor_WithLengthGreaterThan32_ShouldThrowException()
    {
        var length = Random.Shared.Next(33, int.MaxValue);
        Assert.Throws<ArgumentOutOfRangeException>(() => new SubnetMask(length));
    }

    [Fact]
    public void ToString_WithLength4_ShouldReturn240dot0dot0dot0()
    {
        // Arrange
        var expected = "240.0.0.0";
        var mask = new SubnetMask(4);
        
        // Act
        var actual = mask.ToString();
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, "128.0.0.0")]
    [InlineData(32, "255.255.255.255")]
    [InlineData(16, "255.255.0.0")]
    [InlineData(24, "255.255.255.0")]
    [InlineData(0, "0.0.0.0")]
    [InlineData(26, "255.255.255.192")]
    [InlineData(20, "255.255.240.0")]
    public void ToString_WithValidLengthInConstructor_ShouldReturnValidStringIPRepresentation(int length, string expected)
    {
        // Arrange
        var mask = new SubnetMask(length);

        // Act
        var actual = mask.ToString();
        // Assert
        Assert.Equal(expected, actual);
    }
}