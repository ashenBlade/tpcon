using Router.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.TpLink.TLWR741ND.Utils.Security;

public abstract class CustomSecurity : Domain.Security
{
    public abstract SecurityDisplayStatus ToDisplayStatus();
}