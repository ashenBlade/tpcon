namespace Router.Commands.CommandLine;

public interface ICommandLineRouterCommandFactory
{
    IRouterCommand CreateRouterCommand(CommandLineContext context);
}