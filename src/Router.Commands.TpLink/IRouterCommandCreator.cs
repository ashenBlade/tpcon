namespace Router.Commands.TpLink;

internal interface IRouterCommandCreator
{
    IRouterCommand CreateRouterCommand(RouterCommandContext context);
}