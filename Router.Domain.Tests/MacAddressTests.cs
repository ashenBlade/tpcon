using Xunit;

namespace Router.Domain.Tests;

public class MacAddressTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        var record = Record.Exception(() => new MacAddress(new byte[] {1, 1, 1, 1, 1, 1}));
        Assert.Null(record);
    }
}