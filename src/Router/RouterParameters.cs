using System.Net;

namespace Router.Domain;

public struct RouterParameters
{
    public static IPAddress DefaultAddress => new IPAddress(new byte[] {192, 168, 0, 1});
    public static string DefaultUsername => "admin";
    public static string DefaultPassword => "admin";
    public RouterParameters()
    {
        Address = DefaultAddress;
        Username = DefaultUsername;
        Password = DefaultPassword;
    }

    public RouterParameters(IPAddress? address = null, string? username = null, string? password = null)
    {
        Address = address ?? DefaultAddress;
        Username = username ?? DefaultUsername;
        Password = password ?? DefaultPassword;
    }

    public IPAddress Address { get; }
    public string Username { get; }
    public string Password { get; }
}