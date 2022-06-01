using Router.TpLink.TLWR741ND.Commands.DisplayStatus;
using Router.TpLink.TLWR741ND.Utils.Security;

namespace Router.Domain.Infrastructure.Security;

public class NoneSecurity : CustomSecurity
{
    public override string Name => "None";

    public override SecurityDisplayStatus ToDisplayStatus()
    {
        return new NoneSecurityDisplayStatus();
    }
}