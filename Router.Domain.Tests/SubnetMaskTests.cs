using System;
using Xunit;

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
}