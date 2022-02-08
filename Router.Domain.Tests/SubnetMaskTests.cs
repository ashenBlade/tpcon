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
    public void Constructor_WithNegativeSubnetMask_ShouldThrowArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SubnetMask(-7));
    }
}