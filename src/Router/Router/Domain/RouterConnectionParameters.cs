using System.Net;

namespace Router.Domain;

public readonly struct RouterConnectionParameters: IEquatable<RouterConnectionParameters>
{
    public static RouterConnectionParameters Default => new(DefaultAddress, DefaultUsername, DefaultPassword);
    public static string DefaultAddressString => "192.168.0.1";
    public static IPAddress DefaultAddress => IPAddress.Parse(DefaultAddressString);
    public static string DefaultUsername => "admin";
    public static string DefaultPassword => "admin";
    public RouterConnectionParameters()
    {
        Address = DefaultAddress;
        Username = DefaultUsername;
        Password = DefaultPassword;
    }

    public RouterConnectionParameters(IPAddress? address = null, string? username = null, string? password = null)
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
        return obj is RouterConnectionParameters routerParameters && 
               Equals(routerParameters);
    }

    public override string ToString()
    {
        return $"{{ Username = \"{Username}\"; Password = \"{Password}\"; Address = \"{Address.ToString()}\"}}";
    }

    public bool Equals(RouterConnectionParameters other)
    {
        return Address.Equals(other.Address)
            && Username == other.Username
            && Password == other.Password;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Address, Username, Password);
    }

    public static bool operator ==(RouterConnectionParameters left, RouterConnectionParameters right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RouterConnectionParameters left, RouterConnectionParameters right)
    {
        return !(left == right);
    }
}