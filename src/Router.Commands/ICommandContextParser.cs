using System.Windows.Input;

namespace Router.Commands;

public interface ICommandLineContextParser
{
    public CommandContext ParseCommandContext(string[] args);
}