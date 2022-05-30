namespace Router.Domain.Infrastructure.Security;

public abstract class WPASecurity : Domain.Security
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

    public SecurityVersion Version { get; }
    public EncryptionType EncryptionType { get; }
    public int GroupKeyUpdatePeriod { get; }
}