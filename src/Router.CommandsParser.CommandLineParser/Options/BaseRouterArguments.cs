using CommandLine;

namespace Router.CommandsParser.CommandLineParser.Options;

public class BaseRouterArguments
{
    [Option('a', "address", Default = "192.168.0.1", HelpText = "Address of router to connect", Hidden = false, Required = false)]
    public string IpAddress { get; set; }

    [Option('u', "username", Default = "admin", HelpText = "Username of user", Hidden = false, Required = false)]
    public string Username { get; set; }

    [Option('p', "password", Default = "admin", HelpText = "Password of router", Hidden = false, Required = false)]
    public string Password { get; set; }
}