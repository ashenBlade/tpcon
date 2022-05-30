using System.ComponentModel;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

[DisplayName(DisplayName)]
public class NoneSecurityDisplayStatus : SecurityDisplayStatus
{
    public const string DisplayName = "No security";
    public override string Name => DisplayName;
}