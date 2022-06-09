using System.ComponentModel;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

[DisplayName(DisplayName)]
public class PersonalSecurityDisplayStatus : WPASecurityDisplayStatus
{
    public const string DisplayName = "WPA/WPA2 Personal"; 
    public override string Name => DisplayName;
    
    [DisplayName("Password")]
    public string Password { get; }

    public PersonalSecurityDisplayStatus(PersonalSecurity security)
        : this(security.Password, security.Version, security.EncryptionType, security.GroupKeyUpdatePeriod)
    { }
    
    public PersonalSecurityDisplayStatus(string password, SecurityVersion version, EncryptionType encryptionType, int groupKeyUpdateInterval) 
        : base(version, encryptionType, groupKeyUpdateInterval)
    {
        Password = password;
    }
}