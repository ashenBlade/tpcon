namespace Router.Commands.TpLink.CommandFactory;

public abstract class BaseTpLinkCommandFactory : IRouterCommandFactory
{
    public abstract IRouterCommand CreateRouterCommand(RouterCommandContext context);
}