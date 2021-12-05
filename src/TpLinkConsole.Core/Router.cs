using System.Net;
using System.Net.Http;

namespace TpLinkConsole.Core
{
    public class Router
    {
        public IPAddress RouterIPAddress { get; }
        public string Username { get; }
        public string Password { get; }

        internal Router(IPAddress ipAddress, string username, string password)
        {
            RouterIPAddress = ipAddress;
            Username = username;
            Password = password;
        }
    }
}