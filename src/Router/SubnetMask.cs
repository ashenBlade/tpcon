using System.Net;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Router.Tests")]
namespace Router.Domain;

public struct SubnetMask
{
    private readonly string _string;
    public IPAddress ToIpAddress() 
        => IPAddress.Parse(_string);
    public int MaskLength { get; }

    public SubnetMask()
    {
        MaskLength = 0;
        _string = "0.0.0.0";
    }

    public static SubnetMask Zero => new();

    public SubnetMask(int maskLength)
    {
        if (maskLength is < 0 or > 32)
            throw new ArgumentOutOfRangeException(nameof(maskLength), maskLength,
                                                  "Mask length should be positive and less or equal to 32");
        MaskLength = maskLength;
        _string = new IPAddress(IpLongFromLength(maskLength))
           .ToString();
    }

    private static IPAddress IPAddressFromMaskLength(int length)
    {
        return new IPAddress(IpLongFromLength(length));
    }

    private static byte ByteMaskFromLength(int length)
    {
        return length switch
               {
                   0 => MaskZero,
                   1 => MaskOne,
                   2 => MaskTwo,
                   3 => MaskThree,
                   4 => MaskFour,
                   5 => MaskFive,
                   6 => MaskSix,
                   7 => MaskSeven,
                   8 => MaskEight,
                   _ => throw new ArgumentOutOfRangeException(nameof(length))
               };
    }

    internal static (long first, long second, long third, long fourth) GetMaskBytes(int length)
    {
        if (length is < 0 or > 32)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        long GetMask(int len, out int leftLength)
        {
            var byteMaskLength = len >= 8
                                     ? 8
                                     : len;
            leftLength = len - byteMaskLength;
            return ByteMaskFromLength(byteMaskLength);
        }
        
        var firstMask = GetMask(length, out length);
        var secondMask = GetMask(length, out length);
        var thirdMask = GetMask(length, out length);
        var fourthMask = GetMask(length, out length);
        return ( firstMask, secondMask, thirdMask, fourthMask );
    }
    
    private static long IpLongFromLength(int length)
    {
        if (length is < 0 or > 32)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        var (first, second, third, fourth) = GetMaskBytes(length);
        
        // Long represented in big-endian form
        return fourth << 24 | third << 16 | second << 8 | first;
    }

    private const int BytesInMask = 4;
    private const byte MaskZero = 0b0000_0000;
    private const byte MaskOne = 0b1000_0000;
    private const byte MaskTwo = 0b1100_0000;
    private const byte MaskThree = 0b1110_0000;
    private const byte MaskFour = 0b1111_0000;
    private const byte MaskFive = 0b1111_1000;
    private const byte MaskSix = 0b1111_1100;
    private const byte MaskSeven = 0b1111_1110;
    private const byte MaskEight = 0b1111_1111;



    private static int GetLengthForMask(byte mask) 
        => mask switch
           {
               MaskEight => 8,
               MaskSeven => 7,
               MaskSix   => 6,
               MaskFive  => 5,
               MaskFour  => 4,
               MaskThree => 3,
               MaskTwo   => 2,
               MaskOne   => 1,
               MaskZero  => 0,
               _         => throw new ArgumentException(null, nameof(mask))
           };

    public static SubnetMask Parse(string ip)
    {
        var numbers = ip.Split('.');
        if (numbers.Length != 4)
        {
            throw new ArgumentException(null, nameof(ip));
        }

        var bytes = numbers
                   .Select(ToIpByte)
                   .ToArray();
        var length = 0;
        var ended = false;
        for (var i = 0; i < BytesInMask; i++)
        {
            var b = bytes[i];
            if (ended)
            {
                if (b is not MaskZero)
                {
                    throw new ArgumentException(null, nameof(ip));
                }

                continue;
            }

            length += GetLengthForMask(b);
            if (b is not MaskEight)
            {
                ended = true;
            }

        }
        return new SubnetMask(length);
    }

    private static byte ToIpByte(string ip)
    {
        if (ip.Length is 0 or > 3)
        {
            throw new ArgumentException(null, nameof(ip));
        }
        var i = int.Parse(ip);
        if (i is < 0 or > 255)
        {
            throw new ArgumentException(null, "ip");
        }

        return ( byte ) i;
    }
    
    public override string ToString()
    {
        return _string;
    }
    
}
