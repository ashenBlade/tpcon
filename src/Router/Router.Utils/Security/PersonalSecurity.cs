namespace Router.Utils.Security;

public class PersonalSecurity : WPASecurity
{
    public override string Name => "WPA/WPA2 Personal";

    public PersonalSecurity(string password, EncryptionType encryption, SecurityVersion version, int groupKeyUpdatePeriod)
    : base(version, encryption, groupKeyUpdatePeriod)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentOutOfRangeException(nameof(password), "Password not provided");
        }
        if (password.Length < 8)
        {
            throw new ArgumentOutOfRangeException(nameof(password), "Password length must be greater than 8");
        }
        
        Password = password;
    }
    public string Password { get; }
}