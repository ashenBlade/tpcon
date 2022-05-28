using Router.Commands;

namespace Router.TpLink;

internal interface IRouterCommandFactory
{
    IRouterCommand CreateRouterCommand(RouterCommandContext context);
}