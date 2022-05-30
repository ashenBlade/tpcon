using System.ComponentModel;
using System.Globalization;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

[DisplayName(DisplayName)]
public class PersonalSecurityDisplayStatus : SecurityDisplayStatus
{
    public const string DisplayName = "Personal security"; 
    public override string Name => DisplayName;

    public string Password { get; }
    public string Type { get; set; }
}