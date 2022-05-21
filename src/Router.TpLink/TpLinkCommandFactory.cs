using Router.Commands;
using Router.TpLink.CommandFactories;
using Router.TpLink.CommandFactories.Lan;
using Router.TpLink.CommandFactories.Root;
using Router.TpLink.CommandFactories.Wlan;

namespace Router.TpLink;

public class TpLinkCommandFactory : Router.Commands.IRouterCommandFactory
{
    private static IEnumerable<InternalTpLinkCommandFactory> DefaultCommands => new InternalTpLinkCommandFactory[]
                                                                                {
                                                                                    new CheckConnectionTpLinkCommandFactory(),
                                                                                    new RefreshTpLinkCommandFactory(),
                                                                                    new WlanCompositeCommandFactory(),
                                                                                    new LanCompositeCommandFactory(),
                                                                                };
    private readonly ITpLinkRouterFactory _routerFactory;
    private readonly List<InternalTpLinkCommandFactory> _factories;
    
    public TpLinkCommandFactory(ITpLinkRouterFactory routerFactory)
        : this(routerFactory, null)
    { }
    
    internal TpLinkCommandFactory(ITpLinkRouterFactory routerFactory, 
                                  IEnumerable<InternalTpLinkCommandFactory>? factories)
    {
        ArgumentNullException.ThrowIfNull(routerFactory);
        _routerFactory = routerFactory;
        _factories = ( factories ?? DefaultCommands ).ToList();
    }
    
    public IRouterCommand CreateRouterCommand(CommandLineContext context)
    {
        var router = _routerFactory.CreateRouter(context.RouterParameters);
        var formatter = context.GetOutputFormatter();
        var routerContext = new RouterCommandContext(router, context.Command, context.Arguments, formatter, context.OutputStyle);
        return new RootTpLinkCommandFactory(_factories)
           .CreateRouterCommand(routerContext);
    }
}