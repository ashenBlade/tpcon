using System.ComponentModel;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

[DisplayName(DisplayName)]
public class NoneSecurityDisplayStatus : SecurityDisplayStatus
{
    public const string DisplayName = "No security";
    public override string Name => DisplayName;
}