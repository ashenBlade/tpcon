using CommandLine;

namespace Router.CommandsParser.CommandLineParser.Options;

[Verb("wlan-ssid-set", HelpText = "Set SSID of network (Display name of WiFi)")]
internal class SetWlanSsidRouterArguments : BaseRouterArguments
{
    [Option("ssid", Required = true)]
    public string Ssid { get; set; }
}