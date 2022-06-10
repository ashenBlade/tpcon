using System.Net;
using JsTypes;
using JsUtils.Implementation;
using Router.Commands.TpLink.TLWR741ND.Status.Lan.NetworkCfg;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Status.Wlan.Security;

public class WlanSecurityRouterStatusExtractor
    : IWlanRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus>

{
    public const int SecurityType = 2;

    public const int EncryptionTypeIndex = 3;
    public const int EncryptionTypeWep = 0;
    public const int EncryptionTypeWpa = 1;
    public const int EncryptionTypePsk = 2;

    public const int PskSecret = 9;
    public const int PskInterval = 11;
    public const int PskCipher = 14;

    public const int RadiusIp = 6;
    public const int RadiusPort = 7;
    public const int RadiusSecret = 8;
    public const int WpaCipher = 13;
    public const int WpaInterval = 15;

    public WlanSecurityRouterStatus ExtractStatus(WlanSecurityPageStatus status)
    {
        var wlan = status.WlanPara;
        var currentSecurity = wlan[SecurityType].GetInt() switch
                              {
                                  0 => ( Domain.Wlan.Security ) new NoneSecurity(),
                                  1 => ExtractWepSecurity(wlan, status.WlanList),
                                  2 => ExtractEnterpriseSecurity(wlan),
                                  3 => ExtractPersonalSecurity(wlan),
                                  _ => throw new ArgumentOutOfRangeException(nameof(wlan))
                              };

        var total = GetTotalStatus(status);
        return new WlanSecurityRouterStatus(currentSecurity, total);
    }

    private List<KeyValuePair<string, string?>> GetTotalStatus(WlanSecurityPageStatus page)
    {
        var para = page.WlanPara;

        string Para(int i) => para[i] is JsString jsString
                                  ? jsString.Value
                                  : para[i].ToString()!;

        string SecOpt(int idx) => para[EncryptionTypeIndex].GetString()[idx].ToString();
        var wlan = page.WlanList;

        string Wlan(int i) => wlan[i] is JsString jsString
                                  ? jsString.Value
                                  : wlan[i].ToString()!;

        var list = new List<KeyValuePair<string, string>>
                   {
                       new("secType", Para(SecurityType)),
                       new("wepSecOpt", SecOpt(EncryptionTypeWep)),
                       new("keytype", para[WepKeyType].ToString()),
                       new("keynum", ( para[CurrentWepKeyIndex].GetInt() - 1 ).ToString()),
                       new("key1", Wlan(0)),
                       new("length1", Wlan(1)),
                       new("key2", Wlan(2)),
                       new("length2", Wlan(3)),
                       new("key3", Wlan(4)),
                       new("length3", Wlan(5)),
                       new("key4", Wlan(6)),
                       new("length4", Wlan(7)),
                       new("wpaSecOpt", SecOpt(EncryptionTypeWpa)),
                       new("wpaCipher", Para(WpaCipher)),
                       new("radiusIp", Para(RadiusIp)),
                       new("radiusPort", Para(RadiusPort)),
                       new("radiusSecret", Para(RadiusSecret)),
                       new("intervalWpa", Para(WpaInterval)),
                       new("pskSecOpt", SecOpt(EncryptionTypePsk)),
                       new("pskCipher", Para(PskCipher)),
                       new("pskSecret", Para(PskSecret)),
                       new("interval", Para(PskInterval))
                   };
        return list;
    }

    private static SecurityVersion GetSecurityVersion(JsArray array, int type) =>
        array[EncryptionTypeIndex].GetString()[type] switch
        {
            '1' => SecurityVersion.WPA,
            '2' => SecurityVersion.WPA2,
            '3' => SecurityVersion.Automatic,
            _   => throw new ArgumentOutOfRangeException(nameof(array))
        };

    private static EncryptionType GetEncryptionType(JsArray wlan, int type) =>
        wlan[type].GetInt() switch
        {
            1 => EncryptionType.Auto,
            2 => EncryptionType.TKIP,
            3 => EncryptionType.AES,
            _ => throw new ArgumentOutOfRangeException(nameof(wlan))
        };

    private PersonalSecurity ExtractPersonalSecurity(JsArray wlan)
    {
        var password = wlan[PskSecret].GetString();
        var groupKeyUpdate = wlan[PskInterval].GetInt();
        var version = GetSecurityVersion(wlan, EncryptionTypePsk);
        var encryption = GetEncryptionType(wlan, PskCipher);
        return new PersonalSecurity(password, encryption, version, groupKeyUpdate);
    }

    private EnterpriseSecurity ExtractEnterpriseSecurity(JsArray wlan)
    {
        var radiusIpString = wlan[RadiusIp].GetString();
        var radiusIp = string.IsNullOrEmpty(radiusIpString)
                           ? IPAddress.None
                           : IPAddress.Parse(radiusIpString);
        var radiusPort = wlan[RadiusPort].GetInt();
        var radiusSecret = wlan[RadiusSecret].GetString();
        var version = GetSecurityVersion(wlan, EncryptionTypeWpa);
        var encryption = GetEncryptionType(wlan, WpaCipher);
        var groupKeyUpdatePeriod = wlan[WpaInterval].GetInt();
        return new EnterpriseSecurity(new RadiusServer(radiusIp, radiusPort, radiusSecret),
                                      version,
                                      encryption,
                                      groupKeyUpdatePeriod);
    }


    public const int CurrentWepKeyIndex = 10;
    public const int WepKeyType = 4;
    public const int DefaultKeysCount = 4;

    private WepSecurity ExtractWepSecurity(JsArray wlan, JsArray list)
    {
        var version = wlan[EncryptionTypeIndex].GetString()[EncryptionTypeWep] switch
                      {
                          '1' => WepType.OpenSystem,
                          '2' => WepType.SharedKey,
                          '3' => WepType.Automatic,
                          _   => throw new ArgumentOutOfRangeException(nameof(EncryptionTypeIndex))
                      };
        var keyFormat = wlan[WepKeyType].GetInt() switch
                        {
                            1 => WepKeyFormat.Hex,
                            2 => WepKeyFormat.ASCII,
                            _ => throw new ArgumentOutOfRangeException(nameof(WepKeyType))
                        };
        var keys = ExtractWepKeys(list, DefaultKeysCount);
        var selectedIndex = ( wlan[CurrentWepKeyIndex].GetInt() - 1 ) / 2;
        return new WepSecurity(keys, keys[selectedIndex], version, keyFormat);
    }

    private static WepKey[] ExtractWepKeys(JsArray array, int count)
    {
        WepKeyEncryption GetWepKeyEncryption(int value) =>
            value switch
            {
                0  => WepKeyEncryption.Disabled,
                5  => WepKeyEncryption.Bit64,
                13 => WepKeyEncryption.Bit128,
                15 => WepKeyEncryption.Bit152,
                _  => throw new ArgumentOutOfRangeException(nameof(WepKeyEncryption))
            };

        var keys = new List<WepKey>();
        for (int i = 0; i < count * 2; i += 2)
        {
            var key = array[i].GetString();
            var format = GetWepKeyEncryption(array[i + 1].GetInt());
            keys.Add(new WepKey(format, key));
        }

        return keys.ToArray();
    }
}