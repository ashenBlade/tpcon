using Router.Commands.Commands;

namespace Router.Commands;

public interface IRouterCommandFactory
{
    IRouterCommand CreateRouterCommand(CommandContext context);
}