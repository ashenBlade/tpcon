using JsTypes;
using JsUtils.Implementation;
using Router.Domain.Infrastructure.Security;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Security;

public  class WlanSecurityRouterStatusExtractor 
    : IWlanRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus>

{
    public const int PskSecret = 9;
    
    public const int EncryptionTypeIndex = 3;
    public const int EncryptionTypeWep = 0;
    public const int EncryptionTypeWpa = 1;
    public const int EncryptionTypePsk = 2;
    
    public const int SecurityType = 2;
    public const int PskCipher = 14;
    public const int Interval = 11;
    
    public WlanSecurityRouterStatus ExtractStatus(WlanSecurityPageStatus status)
    {
        var wlan = status.WlanPara;
        var security = wlan[SecurityType].GetInt() switch
                       {
                           0 => (Domain.Security) new NoneSecurity(),
                           1 => throw new NotImplementedException("WEP security is not supported yet"),
                           2 => ExtractEnterpriseSecurity(wlan),
                           3 => ExtractPersonalSecurity(wlan),
                           _ => throw new ArgumentOutOfRangeException(nameof(wlan))
                       };
        return new WlanSecurityRouterStatus(security);
    }

    private PersonalSecurity ExtractPersonalSecurity(JsArray wlan)
    {
        var password = wlan[PskSecret].GetString();
        var groupKeyUpdate = wlan[Interval].GetInt();
        var version = wlan[EncryptionTypeIndex].GetString()[EncryptionTypePsk] switch
                      {
                          '0' => SecurityVersion.Automatic,
                          '1' => SecurityVersion.WPA,
                          '2' => SecurityVersion.WPA2,
                          _ => throw new ArgumentOutOfRangeException(nameof(wlan))
                      };
        var encryption = wlan[PskCipher].GetInt() switch
                         {
                             1 => EncryptionType.Auto,
                             2 => EncryptionType.TKIP,
                             3 => EncryptionType.AES,
                             _ => throw new ArgumentOutOfRangeException(nameof(wlan))
                         };
        return new PersonalSecurity(password, encryption, version, groupKeyUpdate);
    }

    private EnterpriseSecurity ExtractEnterpriseSecurity(JsArray wlan)
    {
        throw new NotImplementedException();
    }
}