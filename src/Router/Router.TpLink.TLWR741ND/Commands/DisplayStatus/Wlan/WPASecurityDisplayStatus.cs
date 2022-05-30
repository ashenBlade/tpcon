using System.ComponentModel;
using Router.Domain.Infrastructure.Security;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

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