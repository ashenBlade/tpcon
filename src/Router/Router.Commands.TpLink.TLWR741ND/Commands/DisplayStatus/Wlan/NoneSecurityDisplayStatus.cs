using System.ComponentModel;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

[DisplayName(DisplayName)]
public class NoneSecurityDisplayStatus : SecurityDisplayStatus
{
    public const string DisplayName = "Нет";
    public override string Name => DisplayName;
}