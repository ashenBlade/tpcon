using CommandLine;

namespace Router.Commands.Implementation.Options;

[Verb("refresh", false, Hidden = false, HelpText = "Refresh router")]
internal class Refresh : RouterParameters
{ }