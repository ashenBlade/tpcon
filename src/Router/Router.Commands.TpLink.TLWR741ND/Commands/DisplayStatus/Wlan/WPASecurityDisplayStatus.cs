using System.ComponentModel;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

public abstract class WPASecurityDisplayStatus : SecurityDisplayStatus
{
    protected WPASecurityDisplayStatus(SecurityVersion version,
                                       EncryptionType encryptionType,
                                       int groupKeyUpdateInterval)
    {
        Version = version;
        EncryptionType = encryptionType;
        GroupKeyUpdateInterval = groupKeyUpdateInterval;
    }

    [DisplayName("Шифрование")]
    public EncryptionType EncryptionType { get; }

    [DisplayName("Версия")]
    public SecurityVersion Version { get; }

    [DisplayName("Время обновления группового ключа")]
    public int GroupKeyUpdateInterval { get; }
}