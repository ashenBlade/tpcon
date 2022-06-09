using System.Net;

namespace Router.Exceptions;

public class RouterUnreachableException : RouterConnectionException
{
    public IPAddress Address { get; }

    public RouterUnreachableException(IPAddress address, string? message = null)
        : base(message ?? GetDefaultMessage(address))
    {
        Address = address;
    }

    private static string GetDefaultMessage(IPAddress address) 
        => $"Could not connect to router at {address}";
}