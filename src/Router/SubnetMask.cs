using System.Diagnostics;
using System.Net;

namespace Router.Domain;

public class SubnetMask
{
    public IPAddress IpAddress { get; }
    public int MaskLength { get; }

    public SubnetMask(int maskLength)
    {
        if (maskLength is < 0 or > 32)
            throw new ArgumentOutOfRangeException(nameof(maskLength), maskLength,
                                                  "Mask length should be positive and less or equal to 32");
        MaskLength = maskLength;
        IpAddress = new IPAddress(GetBytesFromLength(MaskLength));
    }

    private const byte MaskZero = 0b0000_0000;
    private const byte MaskOne = 0b1000_0000;
    private const byte MaskTwo = 0b1100_0000;
    private const byte MaskThree = 0b1110_0000;
    private const byte MaskFour = 0b1111_0000;
    private const byte MaskFive = 0b1111_1000;
    private const byte MaskSix = 0b1111_1100;
    private const byte MaskSeven = 0b1111_1110;
    private const byte MaskEight = 0b1111_1111;

    private static readonly byte[] MaskForLength =
    {
        MaskZero, MaskOne, MaskTwo, MaskThree, MaskFour, MaskFive, MaskSix, MaskSeven, MaskEight
    };
    
    private static byte[] GetBytesFromLength(int length)
    {
        const int bytesInMask = 4;
        var bytes = new byte[bytesInMask];
        var full = length / 8;
        var semi = length % 8;
        var i = 0;
        for (; i < full; i++)
        {
            bytes[i] = MaskEight;
        }

        if (i < bytesInMask)
        {
            bytes[i] = MaskForLength[semi];
            i++;
        }

        for (; i < bytesInMask; i++)
        {
            bytes[i] = MaskZero;
        }

        return bytes;
    }

    public override string ToString()
    {
        return IpAddress.ToString();
    }
}