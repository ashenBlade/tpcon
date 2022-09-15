using System.ComponentModel;
using System.Net;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

[DisplayName(DisplayName)]
public class EnterpriseSecurityDisplayStatus : WPASecurityDisplayStatus
{
    public const string DisplayName = "WPA/WPA2 Enterprise";
    public override string Name => DisplayName;

    [DisplayName("Radius адрес")]
    public IPAddress RadiusAddress { get; }

    [DisplayName("Radius порт")]
    public int RadiusPort { get; }

    [DisplayName("Radius пароль")]
    public string RadiusPassword { get; }

    public EnterpriseSecurityDisplayStatus(EnterpriseSecurity security)
        : this(security.Version,
               security.EncryptionType,
               security.GroupKeyUpdatePeriod,
               security.Radius.Password,
               security.Radius.Port,
               security.Radius.Address)
    {
    }

    public EnterpriseSecurityDisplayStatus(SecurityVersion version,
                                           EncryptionType encryptionType,
                                           int groupKeyUpdateInterval,
                                           string radiusPassword,
                                           int radiusPort,
                                           IPAddress radiusAddress)
        : base(version, encryptionType, groupKeyUpdateInterval)
    {
        RadiusPassword = radiusPassword;
        RadiusPort = radiusPort;
        RadiusAddress = radiusAddress;
    }
}