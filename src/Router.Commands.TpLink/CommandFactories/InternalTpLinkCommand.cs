namespace Router.Commands.TpLink.CommandFactories;

internal abstract class InternalTpLinkCommand : IRouterCommandFactory
{
    public string Name { get; }

    public InternalTpLinkCommand(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
    }
    public abstract IRouterCommand CreateRouterCommand(CommandContext context);
    public abstract void WriteHelpTo(TextWriter writer);
}