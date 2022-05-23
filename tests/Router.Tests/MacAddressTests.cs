using Router.Domain;
using Xunit;

namespace Router.Tests;

public class MacAddressTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        var record = Record.Exception(() => new MacAddress(new byte[] {1, 1, 1, 1, 1, 1}));
        Assert.Null(record);
    }

    [Theory]
    [InlineData("11-11-11-11-11-11")]
    [InlineData("A1-FF-1F-11-01-21")]
    [InlineData("01-CB-1E-C5-21-66")]
    [InlineData("00-2B-8E-03-00-AF")]
    public void StaticParse_WithValidMacAddressString_ShouldParseExactly(string representation)
    {
        var mac = MacAddress.Parse(representation);
        Assert.NotNull(mac);
        Assert.Equal(mac.ToString(), representation);
    }
}