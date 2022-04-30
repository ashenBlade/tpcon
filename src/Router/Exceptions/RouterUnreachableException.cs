namespace Router.Domain.Exceptions;

public class RouterUnreachableException : RouterException
{
    public string Address { get; }

    public RouterUnreachableException(string address, string? message = null)
        : base(message)
    {
        Address = address;
    }
}