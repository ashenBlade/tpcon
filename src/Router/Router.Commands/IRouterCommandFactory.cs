namespace Router.Commands;

public interface IRouterCommandFactory
{
    IRouterCommand CreateRouterCommand(CommandLineContext context);
}