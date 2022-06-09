using System;
using System.Collections.Generic;
using System.Linq;
using Router.Domain.Lan;
using Xunit;

namespace Router.Domain.Tests;

public class SubnetMaskTests
{
    private struct MaskTempStruct
    {
        public string IP { get; set; }
        public int Length { get; set; }
    }
    public static IEnumerable<object[]> AllMasks => GetAllMasks();

    private static MaskTempStruct ToStruct(object[] array) 
        => new()
           {
               IP = ( string ) array[0], Length = ( int ) array[1],
           };
    
    private static IEnumerable<object[]> GetAllMasks()
    {
        string ToIpString(long first, long second, long third, long fourth) 
            => $"{first}.{second}.{third}.{fourth}";
        
        for (int length = 0; length < 33; length++)
        {
            var (first, second, third, fourth) = SubnetMask.GetMaskBytes(length);
            yield return new object[] {ToIpString(first, second, third, fourth), length};
        }
        
    }

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

    [Theory(DisplayName = "Constructor; With valid mask length; Should create new instance")]
    [MemberData(nameof(AllMasks))]
    public void ToString_WithValidLengthInConstructor_ShouldReturnValidStringIPRepresentation(string expected, int length)
    {
        var mask = new SubnetMask(length);

        var actual = mask.ToString();
        
        Assert.Equal(expected, actual);
    }

    [Fact(DisplayName = "Parse; With 255.255.255.255; Should return mask with 32 length")]
    public void Parse_With255255255255()
    {
        const int expectedMaxLength = 32;
        var actual = SubnetMask.Parse("255.255.255.255").MaskLength;
        Assert.Equal(expectedMaxLength, actual);
    }
    
    [Fact(DisplayName = "Parse; With 0.0.0.0; Should return 0.0.0.0 mask")]
    public void Parse_WithZeroMask()
    {
        const int zeroLength = 0;
        var actual = SubnetMask.Parse("0.0.0.0");
        Assert.Equal(zeroLength, actual.MaskLength);
    }

    public static IEnumerable<object[]> FullOctets => AllMasks
       .Where(arr => ToStruct(arr).Length % 8 == 0);

    [Theory(DisplayName = "Parse; With octets 255; Should parse correctly")]
    [MemberData(nameof(FullOctets))]
    public void Parse_WithFullOctetsSet(string ip, int length)
    {
        var actual = SubnetMask.Parse(ip);
        Assert.Equal(length, actual.MaskLength);
    }

    public static IEnumerable<object[]> FloatingMaskEnd => AllMasks
       .Where(arr => ToStruct(arr).Length % 8 != 0);

    [Theory(DisplayName = "Parse; With mask end in octet; Should parse correctly")]
    [MemberData(nameof(FloatingMaskEnd))]
    public void Parse_WithMaskEndInOctet(string ip, int length)
    {
        var actual = SubnetMask.Parse(ip);
        Assert.Equal(length, actual.MaskLength);
    }


    [Theory(DisplayName = "Parse; With invalid ip string representation; Should throw argument exception")]
    [InlineData("192.1.0.0")]
    [InlineData("255.255.255.1")]
    [InlineData("127.0.0.1")]
    [InlineData("123.123.123.123")]
    [InlineData("192.168.0.1")]
    [InlineData("255.245.0.0")]
    public void Parse_WithInvalidIpMask_ShouldThrowException(string ip)
    {
        Assert.Throws<ArgumentException>(() => SubnetMask.Parse(ip));
    }

    [Theory(DisplayName = "ToString; Should return correct representation in IP address form")]
    [MemberData(nameof(AllMasks))]
    public void ToString_ShouldReturnCorrectRepresentation(string ip, int length)
    {
        var actual = new SubnetMask(length).ToString();
        Assert.Equal(ip, actual);
    }
}