using System.Net;

namespace Router.Domain;

public struct RouterParameters
{
    public IPAddress IpAddress { get; set; } = new IPAddress(new byte[] {192, 168, 0, 1});
    public string Username { get; set; } = "admin";
    public string Password { get; set; } = "admin";

}