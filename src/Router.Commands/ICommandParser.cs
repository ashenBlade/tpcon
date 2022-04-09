using System.Windows.Input;

namespace Router.Commands;

public interface ICommandParser
{
    public IRouterCommand ParseCommand(string[] commandLineArguments);
}