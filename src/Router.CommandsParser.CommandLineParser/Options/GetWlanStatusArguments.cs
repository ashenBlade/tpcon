using CommandLine;
using Router.Commands.Commands;

namespace Router.CommandsParser.CommandLineParser.Options;

[Verb("wlan-status", false, HelpText = "Get WAN information of router", Hidden = false)]
internal class GetWlanStatusArguments : BaseRouterArguments
{ }