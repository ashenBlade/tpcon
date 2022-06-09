using System.ComponentModel;

namespace Router.Utils.Security;

public abstract class WPASecurity : Domain.Wlan.Security
{
    protected WPASecurity(SecurityVersion version, EncryptionType encryptionType, int groupKeyUpdatePeriod)
    {
        if (GroupKeyUpdatePeriod is < 30 and not 0)
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