using Router.Commands.Commands;
using Router.Commands.TpLink.CommandFactories;
using Router.Commands.TpLink.CommandFactories.Wlan;
using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;
using Router.Domain;

namespace Router.Commands.TpLink;

public class TpLinkCommandFactory : IRouterCommandFactory
{
    private IEnumerable<InternalTpLinkCommand> Factories { get; }

    public TpLinkCommandFactory()
    : this(Array.Empty<InternalTpLinkCommand>())
    { }

    internal TpLinkCommandFactory(IEnumerable<InternalTpLinkCommand> factories)
    {
        Factories = factories;
    }
    
    public IRouterCommand CreateRouterCommand(CommandContext context)
    {
        return new RootTpLinkCommand()
           .CreateRouterCommand(context);
    }
}