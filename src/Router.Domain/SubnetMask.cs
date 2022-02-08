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
        IpAddress = new IPAddress(0);
    }
}