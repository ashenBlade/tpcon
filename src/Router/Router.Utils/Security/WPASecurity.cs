using System.ComponentModel;

namespace Router.Utils.Security;

public abstract class WPASecurity : Domain.Wlan.Security
{
    public const int NoGroupKeyUpdatePeriod = 0;

    protected WPASecurity(SecurityVersion version,
                          EncryptionType encryptionType,
                          int groupKeyUpdatePeriod = NoGroupKeyUpdatePeriod)
    {
        if (groupKeyUpdatePeriod is < 30 and not NoGroupKeyUpdatePeriod)
        {
            throw new ArgumentOutOfRangeException(nameof(groupKeyUpdatePeriod),
                                                  groupKeyUpdatePeriod,
                                                  "Период обновления группового ключа должен быть больше 30 или 0");
        }

        Version = version;
        EncryptionType = encryptionType;
        GroupKeyUpdatePeriod = groupKeyUpdatePeriod;
    }

    [DisplayName("WPA версия")]
    public SecurityVersion Version { get; }

    [DisplayName("Тип шифрования")]
    public EncryptionType EncryptionType { get; }

    [DisplayName("Период обновления группового ключа")]
    public int GroupKeyUpdatePeriod { get; }
}