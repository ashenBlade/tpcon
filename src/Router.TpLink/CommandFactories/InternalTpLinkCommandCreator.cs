using Router.Commands;

namespace Router.TpLink.CommandFactories;

internal abstract class InternalTpLinkCommandCreator : IRouterCommandCreator
{
    public string Name { get; }

    public InternalTpLinkCommandCreator(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
    }
    public abstract IRouterCommand CreateRouterCommand(RouterCommandContext context);
}