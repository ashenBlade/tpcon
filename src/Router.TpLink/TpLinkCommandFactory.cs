using Router.Commands;
using Router.TpLink.CommandCreator;

namespace Router.TpLink;

public abstract class TpLinkCommandFactory : Commands.IRouterCommandFactory
{
    private readonly ITpLinkRouterFactory _routerFactory;
    private readonly TpLinkCommandCreator[] _creators;

    protected TpLinkCommandFactory(ITpLinkRouterFactory routerFactory,
                                   IEnumerable<TpLinkCommandCreator> creators)
    {
        ArgumentNullException.ThrowIfNull(routerFactory);
        ArgumentNullException.ThrowIfNull(creators);
        _routerFactory = routerFactory;
        _creators = creators.ToArray();
    }

    public IRouterCommand CreateRouterCommand(CommandLineContext context)
    {
        var router = _routerFactory.CreateRouter(context.RouterParameters);
        var formatter = context.GetOutputFormatter();
        var routerContext = new RouterCommandContext(router, context.Command, context.Arguments, formatter, context.OutputStyle);
        return new RootTpLinkCommandCreator(_creators)
           .CreateRouterCommand(routerContext);
    }
}