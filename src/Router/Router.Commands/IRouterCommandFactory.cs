namespace Router.Commands;

public interface IRouterCommandFactory
{
    IRouterCommand CreateRouterCommand(RouterCommandContext context);
}