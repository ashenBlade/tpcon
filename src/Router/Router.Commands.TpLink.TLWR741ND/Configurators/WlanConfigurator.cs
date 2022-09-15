using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using Router.Commands.TpLink.TLWR741ND.Status;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan.Network;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan.Security;
using Router.Commands.TpLink.TLWR741ND.Utils;
using Router.Domain.Wlan;
using Router.Exceptions.NotSupported;
using Router.Utils.Security;

[assembly: InternalsVisibleTo("Router.Commands.TpLink.TLWR741ND.Tests")]

namespace Router.Commands.TpLink.TLWR741ND.Configurators;

public class WlanConfigurator : BaseWlanConfigurator
{
    private static string WlanNetworkPagePath =>
        "userRpm/WlanNetworkRpm.htm";

    private static string WlanSecurityPagePath =>
        "userRpm/WlanSecurityRpm.htm";

    private readonly IRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus> _networkExtractor;
    private readonly IRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus> _securityExtractor;

    public WlanConfigurator(IRouterHttpMessageSender messageSender,
                            IRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus> networkExtractor,
                            IRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus> securityExtractor)
        : base(messageSender)
    {
        _networkExtractor = networkExtractor;
        _securityExtractor = securityExtractor;
    }

    public override async Task<WlanParameters> GetStatusAsync()
    {
        var networkStatus = await GetWlanNetworkRouterStatusAsync();
        var securityStatus = await GetSecurityRouterStatusAsync();
        return new WlanParameters(networkStatus.SSID,
                                  networkStatus.Enabled,
                                  networkStatus.Channel,
                                  networkStatus.Rate,
                                  securityStatus.CurrentSecurity);
    }

    private async Task<WlanNetworkRouterStatus> GetWlanNetworkRouterStatusAsync()
    {
        var network = await GetPageVariablesAsync(WlanNetworkPagePath);
        var networkWlanPara = GetRequiredArray(network, "wlanPara", WlanNetworkPagePath);
        var rateTable = GetRequiredArray(network, "rateTable", WlanNetworkPagePath);
        return _networkExtractor.ExtractStatus(new WlanNetworkPageStatus(networkWlanPara, rateTable));
    }

    private async Task<WlanSecurityRouterStatus> GetSecurityRouterStatusAsync()
    {
        var security = await GetPageVariablesAsync(WlanSecurityPagePath);
        var wlanPara = GetRequiredArray(security, "wlanPara", WlanSecurityPagePath);
        var wlanList = GetRequiredArray(security, "wlanList", WlanSecurityPagePath);
        return _securityExtractor.ExtractStatus(new WlanSecurityPageStatus(wlanPara, wlanList));
    }

    private Task SetWirelessRadioStatusInternal(bool active)
    {
        return MessageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanNetworkRpm.htm",
                                                                    new KeyValuePair<string, string>[]
                                                                    {
                                                                        new("ap", active
                                                                                      ? "1"
                                                                                      : "0"),
                                                                        new("Save", "Save")
                                                                    }));
    }

    public override Task EnableAsync()
    {
        return SetWirelessRadioStatusInternal(true);
    }

    public override Task DisableAsync()
    {
        return SetWirelessRadioStatusInternal(false);
    }

    public override async Task SetSecurityAsync(Security security) =>
        await MessageSender.SendMessageAndSaveAsync(WlanSecurityPagePath, await GetTotalSecuritySendList(security));


    private async Task<IEnumerable<KeyValuePair<string, string>>> GetTotalSecuritySendList(Security security)
    {
        var total = ( await GetSecurityRouterStatusAsync() )
                   .TotalStatusValues
                   .ToDictionary(v => v.Key, v => v.Value);
        var update = GetWlanSecurityKeyValuePairs(security);
        foreach (var up in update)
        {
            total[up.Key] = up.Value;
        }

        return total;
    }

    internal static IEnumerable<KeyValuePair<string, string>> GetWlanSecurityKeyValuePairs(Security security)
    {
        return security switch
               {
                   PersonalSecurity personal     => ExtractPersonalSecurityValues(personal),
                   EnterpriseSecurity enterprise => ExtractEnterpriseSecurityValues(enterprise),
                   WepSecurity wep               => ExtractWepSecurityValues(wep),
                   NoneSecurity                  => ExtractNoneSecurityValues(),
                   null                          => throw new ArgumentNullException(nameof(security)),
                   _ => throw new
                            FunctionalityNotSupportedRouterException($"Security {security.Name} is not supported")
               };
    }


    private static IEnumerable<KeyValuePair<string, string>> ExtractPersonalSecurityValues(PersonalSecurity personal)
    {
        yield return new KeyValuePair<string, string>("secType", "3");
        yield return new KeyValuePair<string, string>("pskSecOpt", ExtractSecurityVersion(personal));
        yield return new KeyValuePair<string, string>("pskCipher", ExtractEncryptionType(personal));
        yield return new KeyValuePair<string, string>("pskSecret", personal.Password);
        yield return new KeyValuePair<string, string>("interval", personal.GroupKeyUpdatePeriod.ToString());
    }

    private static string ExtractSecurityVersion(WPASecurity personal)
    {
        return personal.Version switch
               {
                   SecurityVersion.WPA       => "1",
                   SecurityVersion.WPA2      => "2",
                   SecurityVersion.Automatic => "3",
                   _ => throw new
                            FunctionalityNotSupportedRouterException($"Тип безопасности ${personal.Version} не поддерживается")
               };
    }

    private static string ExtractEncryptionType(WPASecurity personal) =>
        personal.EncryptionType switch
        {
            EncryptionType.Auto => "1",
            EncryptionType.TKIP => "2",
            EncryptionType.AES  => "3",
            _ => throw new
                     FunctionalityNotSupportedRouterException($"Шифрование ${personal.EncryptionType} не поддерживается")
        };

    private static KeyValuePair<string, string> Pair(string s1, string s2) => new(s1, s2);

    private static IEnumerable<KeyValuePair<string, string>> ExtractEnterpriseSecurityValues(
        EnterpriseSecurity enterprise)
    {
        yield return Pair("secType", "2");
        yield return Pair("wpaCipher", ExtractEncryptionType(enterprise));
        yield return Pair("wpaSecOpt", ExtractSecurityVersion(enterprise));
        yield return Pair("intervalWpa", enterprise.GroupKeyUpdatePeriod.ToString());
        var radius = enterprise.Radius;
        yield return Pair("radiusIp", radius.Address.ToString());
        yield return Pair("radiusPort", radius.Port.ToString());
        yield return Pair("radiusSecret", radius.Password);
    }

    private static IEnumerable<KeyValuePair<string, string>> ExtractNoneSecurityValues()
    {
        yield return Pair("secType", "0");
    }

    private static IEnumerable<KeyValuePair<string, string>> ExtractWepSecurityValues(WepSecurity wep)
    {
        yield return Pair("secType", "1");
        yield return Pair("wepSecOpt", wep.Type switch
                                       {
                                           WepType.OpenSystem => "1",
                                           WepType.SharedKey  => "2",
                                           WepType.Automatic  => "3",
                                           _ => throw new
                                                    FunctionalityNotSupportedRouterException($"WEP тип {wep.Type} не поддерживается")
                                       });
        yield return Pair("keytype", wep.Format switch
                                     {
                                         WepKeyFormat.Hex   => "1",
                                         WepKeyFormat.ASCII => "2",
                                         _ => throw new
                                                  FunctionalityNotSupportedRouterException($"Тип WEP ключа {wep.Format} не поддерживается")
                                     });

        // Override first key
        // Wep security no longer used wisely
        // Added just for compability
        yield return Pair("key1", wep.Selected.Key);
        yield return Pair("length1", wep.Selected.Encryption switch
                                     {
                                         WepKeyEncryption.Disabled => "0",
                                         WepKeyEncryption.Bit64    => "5",
                                         WepKeyEncryption.Bit128   => "13",
                                         WepKeyEncryption.Bit152   => "16",
                                         _ => throw new
                                                  FunctionalityNotSupportedRouterException($"Длина WEP ключа {wep.Selected.Encryption} не поддерживается")
                                     });
        for (int i = 2; i < 5; i++)
        {
            yield return Pair($"key{i}", string.Empty);
            yield return Pair($"length{i}", string.Empty);
        }
    }

    public override Task SetSSIDAsync(string ssid)
    {
        if (ssid is null or {Length: < 1 or > 32})
        {
            throw new ArgumentOutOfRangeException(nameof(ssid), "Длина SSID должна быть от 1 до 32 символов");
        }

        return MessageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanNetworkRpm.htm",
                                                                    new KeyValuePair<string, string>[]
                                                                    {
                                                                        new("ap", "1"),
                                                                        new("ssid1", HttpUtility.UrlEncode(ssid)),
                                                                        new("broadcast", "2"), new("Save", "Save")
                                                                    }));
    }
}