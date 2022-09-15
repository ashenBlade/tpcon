using System.Net;

namespace Router.Utils.Security;

public class RadiusServer
{
    public const int DefaultPort = 1812;
    private const int MaxPasswordLength = 252;

    public RadiusServer(string password, IPAddress address, int port = DefaultPort)
    {
        ArgumentNullException.ThrowIfNull(address);
        if (port < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(port), "Порт не может быть отрицательным");
        }

        if (password is null or {Length: 0 or > MaxPasswordLength})
        {
            throw new ArgumentOutOfRangeException(nameof(password), "Длина пароля должна быть в диапазоне от 1 до 63");
        }

        if (port is < 0 or > 65535)
        {
            throw new ArgumentOutOfRangeException(nameof(port), "Порт должен быть в диапазоне от 1 до 65535");
        }

        Address = address;
        Port = port is 0
                   ? DefaultPort
                   : port;
        Password = password;
    }

    public IPAddress Address { get; }
    public int Port { get; }
    public string Password { get; }
}