namespace Router.Commands.TpLink.CommandFactory;

public abstract class TpLinkCommandFactory : IRouterCommandFactory
{
    public string Name { get; }

    public TpLinkCommandFactory(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
    }
    public abstract IRouterCommand CreateRouterCommand(RouterCommandContext context);
}