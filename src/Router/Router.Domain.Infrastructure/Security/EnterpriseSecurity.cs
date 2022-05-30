using System.Net;

namespace Router.Domain.Infrastructure.Security;

public class EnterpriseSecurity : WPASecurity
{
    public EnterpriseSecurity(RadiusServer radius, SecurityVersion version, EncryptionType encryptionType, int groupKeyUpdatePeriod) 
        : base(version, encryptionType, groupKeyUpdatePeriod)
    {
        ArgumentNullException.ThrowIfNull(radius);
        Radius = radius;
    }

    public RadiusServer Radius { get; }
}