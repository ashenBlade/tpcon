using System.Net;

namespace Router.Domain;

public class SubnetMask
{
    public IPAddress IpAddress { get; }
    public int MaskLength { get; }

    public SubnetMask(int maskLength)
    {
        MaskLength = maskLength;
        IpAddress = new IPAddress(0);
    }
}