using System.Net;

namespace Router.Exceptions;

public class InvalidRouterCredentialsException : RouterConnectionException
{
    public string Username { get; }
    public string Password { get; }
    public IPAddress Address { get; }

    public InvalidRouterCredentialsException(IPAddress address, string username, string password, string? message = null)
    : base(message ?? GetDefaultMessage(address, username, password))
    {
        Address = address;
        Username = username;
        Password = password;
    }

    private static string GetDefaultMessage(IPAddress address, string username, string password) 
        => $"Could not connect to router at {address} with username=\"{username}\" and password=\"{password}\"";
}