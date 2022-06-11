namespace Router.Utils.Security;

public class PersonalSecurity : WPASecurity
{
    public const int NoKeyUpdatePeriod = 0;
    public override string Name => "WPA/WPA2 Personal";

    public PersonalSecurity(string password, EncryptionType encryptionType, SecurityVersion version)
        : this(password, encryptionType, version, NoKeyUpdatePeriod)
    {
    }

    public PersonalSecurity(string password,
                            EncryptionType encryption,
                            SecurityVersion version,
                            int groupKeyUpdatePeriod)
        : base(version, encryption, groupKeyUpdatePeriod)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentOutOfRangeException(nameof(password), "Password not provided");
        }

        if (password.Length is < 8 or > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(password), "Password length must be between 8 and 64");
        }

        if (groupKeyUpdatePeriod is < 30 and not 0)
        {
            throw new ArgumentOutOfRangeException(nameof(groupKeyUpdatePeriod),
                                                  "Group key update period min value is 30 or 0 (no update)");
        }

        Password = password;
    }

    public string Password { get; }
}