using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Lan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Lan;

internal class LanCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    private static IEnumerable<KeyValuePair<string, Func<BaseTpLinkCommandFactory>>> GetLanCommands(
        ILanConfigurator lan)
    {
        yield return new("status", () => new GetLanStatusCommandFactory(lan));
        yield return new("ip", () => new SetIpAddressLanCommandFactory(lan));
        yield return new("mask", () => new SetSubnetMaskLanCommandFactory(lan));
    }

    public LanCompositeCommandFactory(ILanConfigurator lan)
        : base(GetLanCommands(lan))
    {
    }
}