using System.Collections.Generic;
using System.Linq;
using Router.Commands.TpLink.TLWR741ND.Configurators;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan.Security;
using Router.Domain.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Tests.Mocks;

public class
    MockWlanSecurityRouterStatusExtractor : IWlanRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus>
{
    public Security Security { get; }

    private static IEnumerable<KeyValuePair<string, string>> DefaultStatus =>
        new KeyValuePair<string, string>[]
        {
            new("secType", "0"), new("wepSecOpt", "1"), new("keytype", "1"), new("keynum", "0"), new("key1", ""),
            new("length1", "0"), new("key2", ""), new("length2", "0"), new("key3", ""), new("length3", "0"),
            new("key4", ""), new("length4", "0"), new("wpaSecOpt", "3"), new("wpaCipher", "1"), new("radiusIp", ""),
            new("radiusPort", ""), new("radiusSecret", ""), new("intervalWpa", "0"), new("pskSecOpt", "3"),
            new("pskCipher", "1"), new("pskSecret", ""), new("interval", "0")
        };

    public MockWlanSecurityRouterStatusExtractor(Security security)
    {
        Security = security;
    }

    public WlanSecurityRouterStatus ExtractStatus(WlanSecurityPageStatus status)
    {
        var replacement = WlanConfigurator.GetWlanSecurityKeyValuePairs(Security);
        var dict = DefaultStatus.ToDictionary(p => p.Key, p => p.Value);
        foreach (var keyValuePair in replacement)
        {
            dict[keyValuePair.Key] = keyValuePair.Value;
        }

        return new WlanSecurityRouterStatus(Security, dict);
    }
}