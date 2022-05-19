using Router.Commands;

namespace Router.TpLink.CommandFactories;

internal abstract class InternalTpLinkCommandFactory : IRouterCommandFactory
{
    public string Name { get; }

    public InternalTpLinkCommandFactory(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
    }
    public abstract IRouterCommand CreateRouterCommand(RouterCommandContext context);
}