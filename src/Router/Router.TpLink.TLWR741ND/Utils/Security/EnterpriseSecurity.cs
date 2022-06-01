using System.Net;
using Router.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.Domain.Infrastructure.Security;

public class EnterpriseSecurity : WPASecurity
{
    public override string Name => "WPA/WPA2 Enterprise";

    public EnterpriseSecurity(RadiusServer radius, SecurityVersion version, EncryptionType encryptionType, int groupKeyUpdatePeriod) 
        : base(version, encryptionType, groupKeyUpdatePeriod)
    {
        ArgumentNullException.ThrowIfNull(radius);
        Radius = radius;
    }

    public override SecurityDisplayStatus ToDisplayStatus()
    {
        return new EnterpriseSecurityDisplayStatus();
    }

    public RadiusServer Radius { get; }
}