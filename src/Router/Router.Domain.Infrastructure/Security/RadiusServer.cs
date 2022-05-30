using System.Net;

namespace Router.Domain.Infrastructure.Security;

public class RadiusServer
{
    private const int DefaultPort = 1812;
    
    public RadiusServer(IPAddress address, int port, string password)
    {
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(address);
        if (port < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(port), "Port must not be negative");
        }
        Address = address;
        Port = port is 0 
                   ? DefaultPort 
                   : port;
        Password = password;
    }

    public IPAddress? Address { get; }
    public int Port { get; }
    public string Password { get; }
}