using System.ComponentModel;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

[DisplayName(DisplayName)]
public class EnterpriseSecurityDisplayStatus : SecurityDisplayStatus
{
    public const string DisplayName = "WPA/WPA2 Enterprise";
    public override string Name => DisplayName;
}