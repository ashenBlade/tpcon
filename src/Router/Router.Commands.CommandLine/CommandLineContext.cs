using Router.Domain;

namespace Router.Commands.CommandLine;

public class CommandLineContext
{
    private int _currentCommandIndex = 0;
    public string[] Command { get; }
    public RouterConnectionParameters RouterConnectionParameters { get; }
    public IDictionary<string, string> Arguments { get; }
    public OutputStyle OutputStyle { get; }

    public CommandLineContext(string[] command, 
                              RouterConnectionParameters routerConnectionParameters, 
                              IDictionary<string, string> arguments, 
                              OutputStyle outputStyle)
    {
        Command = command;
        RouterConnectionParameters = routerConnectionParameters;
        Arguments = arguments;
        OutputStyle = outputStyle;
    }

    public string? CurrentCommand =>
        _currentCommandIndex < Command.Length
            ? Command[_currentCommandIndex]
            : null;

    public string? NextCommand => _currentCommandIndex + 1 < Command.Length
                                      ? Command[_currentCommandIndex + 1]
                                      : null;
    public bool HasNextCommand => _currentCommandIndex < Command.Length;

    public bool MoveNext()
    {
        if (_currentCommandIndex + 1 >= Command.Length)
        {
            return false;
        }

        _currentCommandIndex++;
        return true;
    }

    public void Deconstruct(out string[] Command,
                            out RouterConnectionParameters RouterConnectionParameters,
                            out IDictionary<string, string> Arguments,
                            out OutputStyle OutputStyle)
    {
        Command = this.Command;
        RouterConnectionParameters = this.RouterConnectionParameters;
        Arguments = this.Arguments;
        OutputStyle = this.OutputStyle;
    }
}