using Router.Commands;

namespace Router.TpLink.CommandCreator;

public abstract class TpLinkCommandCreator : IRouterCommandFactory
{
    public string Name { get; }

    public TpLinkCommandCreator(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
    }
    public abstract IRouterCommand CreateRouterCommand(RouterCommandContext context);
}