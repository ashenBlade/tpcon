using Router.Commands.Commands;
using Router.Commands.TpLink.CommandFactories;
using Router.Commands.TpLink.CommandFactories.Wlan;
using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;
using Router.Domain;

namespace Router.Commands.TpLink;

public class TpLinkCommandFactory : IRouterCommandFactory
{
    private IEnumerable<InternalTpLinkCommandCreator> Factories { get; }

    public TpLinkCommandFactory()
    : this(Array.Empty<InternalTpLinkCommandCreator>())
    { }

    internal TpLinkCommandFactory(IEnumerable<InternalTpLinkCommandCreator> factories)
    {
        Factories = factories;
    }
    
    public IRouterCommand CreateRouterCommand(CommandLineContext lineContext)
    {
        var router = new TLWR741NDTpLinkRouter(lineContext.RouterParameters);
        var routerContext = new RouterCommandContext(router, lineContext.Command);
        return new RootTpLinkCommandCreator()
           .CreateRouterCommand(routerContext);
    }
}