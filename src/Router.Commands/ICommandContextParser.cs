using System.Windows.Input;

namespace Router.Commands;

public interface ICommandLineContextParser
{
    public CommandLineContext ParseCommandLineContext(string[] args);
}