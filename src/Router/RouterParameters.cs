using System.Net;

namespace Router.Domain;

public struct RouterParameters: IEquatable<RouterParameters>
{
    public static RouterParameters Default => new(DefaultAddress, DefaultUsername, DefaultPassword);
    public static IPAddress DefaultAddress => new(new byte[] {192, 168, 0, 1});
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

    public override bool Equals(object? obj)
    {
        return obj is RouterParameters routerParameters && 
               Equals(routerParameters);
    }

    public override string ToString()
    {
        return $"{{ Username = \"{Username}\"; Password = \"{Password}\"; Address = \"{Address.ToString()}\"}}";
    }

    public bool Equals(RouterParameters other)
    {
        return Address.Equals(other.Address)
            && Username == other.Username
            && Password == other.Password;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Address, Username, Password);
    }
}