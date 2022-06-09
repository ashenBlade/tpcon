using System.ComponentModel;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

public abstract class WPASecurityDisplayStatus : SecurityDisplayStatus
{
    protected WPASecurityDisplayStatus(SecurityVersion version, EncryptionType encryptionType, int groupKeyUpdateInterval)
    {
        Version = version;
        EncryptionType = encryptionType;
        GroupKeyUpdateInterval = groupKeyUpdateInterval;
    }

    [DisplayName("Encryption")]
    public EncryptionType EncryptionType { get; }
    
    [DisplayName("Version")]
    public SecurityVersion Version { get; }
    
    [DisplayName("Group update key interval")]
    public int GroupKeyUpdateInterval { get; }
}