using System.Globalization;
using System.Text.RegularExpressions;

namespace Router.Domain.Lan;

public class MacAddress
{
    private readonly byte[] _octets;
    private string? _representation;
    public MacAddress(IReadOnlyList<byte> octets)
    {
        if (octets.Count != 6)
        {
            throw new ArgumentOutOfRangeException(nameof(octets.Count), octets.Count,
                                                  "There is must be 6 bytes in MAC address array length");
        }

        _octets = new[] {octets[0], octets[1], octets[2], octets[3], octets[4], octets[5] };
    }
    
    
    
    public static MacAddress Parse(string mac)
    {
        if (string.IsNullOrWhiteSpace(mac))
        {
            throw new ArgumentOutOfRangeException(nameof(mac), mac, "Mac address can not be null or white space");
        }

        if (mac.Length != 17)
        {
            throw new ArgumentOutOfRangeException(nameof(mac), mac, "Mac address length must be 17");
        }

        const string MacAddressRegex = @"^[\dABCDEF]{2}(-[\dABCDEF]{2}){5}$";
        var regex = new Regex(MacAddressRegex);
        if (!regex.IsMatch(mac))
        {
            throw new ArgumentOutOfRangeException(nameof(mac), mac, $"Mac address does not satisfy representation: {MacAddressRegex}");
        }
        
        return new MacAddress(mac.Split('-').Select(hex => byte.Parse(hex, NumberStyles.HexNumber)).ToArray());
    }

    public bool Unicast => ( _octets[0] & 0b0000_0001 ) == 0;
    public bool Multicast => !Unicast;

    public bool GloballyUnique => ( _octets[0] & 0b0000_0010 ) == 0;
    public bool LocallyAdministered => !GloballyUnique;

    public override string ToString()
    {
        return _representation ??= GetRepresentation(_octets);
    }

    private string GetRepresentation(byte[] octets)
    {
        return octets.Select(octet => $"{octet:X2}").Aggregate((s, n) => $"{s}-{n}");
    }
}