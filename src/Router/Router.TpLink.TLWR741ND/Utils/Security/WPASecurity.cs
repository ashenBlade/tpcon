using System.ComponentModel;
using Router.TpLink.TLWR741ND.Utils.Security;

namespace Router.Domain.Infrastructure.Security;

public abstract class WPASecurity : CustomSecurity
{
    protected WPASecurity(SecurityVersion version, EncryptionType encryptionType, int groupKeyUpdatePeriod)
    {
        if (GroupKeyUpdatePeriod is < 30 and not 0 )
        {
            throw new ArgumentOutOfRangeException(nameof(groupKeyUpdatePeriod),
                                                  "Group key update period must be greater than 30 or 0");
        }
        Version = version;
        EncryptionType = encryptionType;
        GroupKeyUpdatePeriod = groupKeyUpdatePeriod;
    }

    [DisplayName("WPA Version")]
    public SecurityVersion Version { get; }
    [DisplayName("Encryption type")]
    public EncryptionType EncryptionType { get; }
    [DisplayName("Group key update period")]
    public int GroupKeyUpdatePeriod { get; }
}