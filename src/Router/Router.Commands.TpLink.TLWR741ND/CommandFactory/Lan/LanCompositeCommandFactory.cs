using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Lan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Lan;

internal class LanCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    private static IEnumerable<TpLink.CommandFactory.TpLinkCommandFactory> GetLanCommands(ILanConfigurator lan)
        => new TpLink.CommandFactory.TpLinkCommandFactory[]
           {
               new GetLanStatusCommandFactory(lan)
           };
    
    public LanCompositeCommandFactory(ILanConfigurator lan)
        : base(GetLanCommands(lan), "lan")
    { }
}