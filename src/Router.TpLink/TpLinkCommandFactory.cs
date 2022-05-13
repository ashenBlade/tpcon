using Router.Commands;
using Router.TpLink.CommandFactories;
using Router.TpLink.CommandFactories.Root;
using Router.TpLink.CommandFactories.Wlan;

namespace Router.TpLink;

public class TpLinkCommandFactory : IRouterCommandFactory
{
    private static IEnumerable<InternalTpLinkCommandCreator> DefaultCommands => new InternalTpLinkCommandCreator[]
                                                                                {
                                                                                    new CheckConnectionTpLinkCommandCreator(),
                                                                                    new RefreshTpLinkCommandCreator(),
                                                                                    new WlanCompositeTpLinkCommandCreator(),
                                                                                };
    private readonly ITpLinkRouterFactory _routerFactory;
    private readonly IRouterHttpMessageSender _messageSender;
    private readonly List<InternalTpLinkCommandCreator> _factories;
    
    public TpLinkCommandFactory(ITpLinkRouterFactory routerFactory, IRouterHttpMessageSender messageSender)
        : this(routerFactory, null, messageSender)
    { }
    
    internal TpLinkCommandFactory(ITpLinkRouterFactory routerFactory, 
                                  IEnumerable<InternalTpLinkCommandCreator>? factories, 
                                  IRouterHttpMessageSender messageSender)
    {
        ArgumentNullException.ThrowIfNull(routerFactory);
        _routerFactory = routerFactory;
        _messageSender = messageSender;
        _factories = ( factories ?? DefaultCommands ).ToList();
    }
    
    public IRouterCommand CreateRouterCommand(CommandLineContext context)
    {
        var router = _routerFactory.CreateRouter(_messageSender);
        var routerContext = new RouterCommandContext(router, context.Command, context.Arguments, context.OutputStyle);
        return new RootTpLinkCommandCreator(_factories)
           .CreateRouterCommand(routerContext);
    }
}