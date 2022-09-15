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
        => $"Не удалось подключиться к роутеру по адресу {address}";
}