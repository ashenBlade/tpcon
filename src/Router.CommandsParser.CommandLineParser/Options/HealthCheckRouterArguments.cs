using CommandLine;

namespace Router.CommandsParser.CommandLineParser.Options;

[Verb("health", false, HelpText = "Check connection to router", Hidden = false)]
internal class HealthCheckRouterArguments : BaseRouterArguments
{ }