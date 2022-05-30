using System.ComponentModel;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

[DisplayName(DisplayName)]
public class EnterpriseSecurityDisplayStatus : SecurityDisplayStatus
{
    public const string DisplayName = "Exterprise security";
    public override string Name => DisplayName;
}