using Router.Commands.TpLink.CommandFactory;
using Router.Domain;

namespace Router.Commands.TpLink;

public abstract class TpLinkCommandFactory : RouterCommandFactory
{
    private readonly CommandFactory.TpLinkCommandFactory[] _creators;

    protected TpLinkCommandFactory(IEnumerable<CommandFactory.TpLinkCommandFactory> creators, RouterConnectionParameters connectionParameters)
        : base(connectionParameters)
    {
        ArgumentNullException.ThrowIfNull(creators);
        _creators = creators.ToArray();
    }
    
    public IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new RootTpLinkCommandFactory(_creators)
           .CreateRouterCommand(context);
    }
}