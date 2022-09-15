using Router.Domain.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

public abstract class SecurityDisplayStatus : TpLink.Commands.DisplayStatus
{
    public abstract string Name { get; }

    public static SecurityDisplayStatus FromSecurity(Security security)
    {
        return security switch
               {
                   PersonalSecurity personal => new PersonalSecurityDisplayStatus(personal),
                   EnterpriseSecurity enterprise => new EnterpriseSecurityDisplayStatus(enterprise),
                   WepSecurity wep => new WepDisplayStatus(wep),
                   NoneSecurity => new NoneSecurityDisplayStatus(),
                   _ => throw new ArgumentOutOfRangeException(nameof(security), "Неизвестный тип безопасности")
               };
    }
}