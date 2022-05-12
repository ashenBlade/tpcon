using System.Net;

namespace Router.Domain.Exceptions;

public class RouterUnreachableException : RouterException
{
    public string Address { get; }

    public RouterUnreachableException(IPAddress address, string? message = null)
        : this(address.ToString(), message)
    { }

    
    public RouterUnreachableException(string address, string? message = null)
        : base(message)
    {
        Address = address;
    }
}