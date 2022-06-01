using System.ComponentModel;
using System.Globalization;
using Router.Domain.Infrastructure.Security;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

[DisplayName(DisplayName)]
public class PersonalSecurityDisplayStatus : WPASecurityDisplayStatus
{
    public const string DisplayName = "WPA/WPA2 Personal"; 
    public override string Name => DisplayName;
    
    [DisplayName("Password")]
    public string Password { get; }

    public PersonalSecurityDisplayStatus(string password, SecurityVersion version, EncryptionType encryptionType, int groupKeyUpdateInterval) : base(version, encryptionType, groupKeyUpdateInterval)
    {
        Password = password;
    }
}