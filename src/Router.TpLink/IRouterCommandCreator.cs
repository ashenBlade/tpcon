using Router.Commands;

namespace Router.TpLink;

internal interface IRouterCommandCreator
{
    IRouterCommand CreateRouterCommand(RouterCommandContext context);
}