using Router.Commands.TpLink.CommandFactory;
using Router.Domain;

namespace Router.Commands.TpLink;

public abstract class TpLinkCommandFactory : RouterCommandFactory
{
    private readonly IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>> _creators;

    protected TpLinkCommandFactory(
        IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>> creators,
        RouterConnectionParameters connectionParameters)
        : base(connectionParameters)
    {
        ArgumentNullException.ThrowIfNull(creators);
        _creators = creators;
    }

    public IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new RootTpLinkCommandFactory(_creators)
           .CreateRouterCommand(context);
    }
}