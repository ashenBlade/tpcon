using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Router.Utils.Security;

public class PersonalSecurity : WPASecurity
{
    public override string Name => "WPA/WPA2 Personal";

    public PersonalSecurity(string password,
                            EncryptionType encryption,
                            SecurityVersion version,
                            int groupKeyUpdatePeriod = NoGroupKeyUpdatePeriod)
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

        Password = password;
    }

    public string Password { get; }
}