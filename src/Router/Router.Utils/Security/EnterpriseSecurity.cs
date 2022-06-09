namespace Router.Utils.Security;

public class EnterpriseSecurity : WPASecurity
{
    public override string Name => "WPA/WPA2 Enterprise";

    public EnterpriseSecurity(RadiusServer radius, SecurityVersion version, EncryptionType encryptionType, int groupKeyUpdatePeriod) 
        : base(version, encryptionType, groupKeyUpdatePeriod)
    {
        ArgumentNullException.ThrowIfNull(radius);
        Radius = radius;
    }

    public RadiusServer Radius { get; }
}