using System.Windows.Input;

namespace Router.Commands;

public interface ICommandParser
{
    public IRouterCommand ParseCommand(IEnumerable<string> commandLineArguments);
}