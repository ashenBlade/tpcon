using CommandLine;

namespace Router.CommandsParser.CommandLineParser.Options;

[Verb("refresh", false, Hidden = false, HelpText = "Refresh router")]
public class RefreshRouterArguments : BaseRouterArguments
{ }