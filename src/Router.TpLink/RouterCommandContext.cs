using Router.Commands;
using Router.Commands.Utils;

namespace Router.TpLink;

public record RouterCommandContext(TpLinkRouter Router, 
                                   IDictionary<string, string> Arguments, 
                                   IOutputFormatter OutputFormatter,
                                   TextWriter OutputWriter,
                                   OutputStyle OutputStyle = OutputStyle.KeyValue)
{
    public IEnumerable<string> Command => _command;
    private readonly string[] _command;

    public RouterCommandContext(TpLinkRouter router, 
                                string[] command, 
                                IDictionary<string, string> arguments,
                                IOutputFormatter formatter,
                                OutputStyle outputStyle = OutputStyle.KeyValue) 
        : this(router, arguments, formatter, Console.Out, outputStyle)
    {
        ArgumentNullException.ThrowIfNull(command);
        _command = command;
    }
    
    public string? CurrentCommand =>
        _currentCommandIndex < _command.Length
            ? _command[_currentCommandIndex]
            : null;

    public string? NextCommand => _currentCommandIndex + 1 < _command.Length
                                      ? _command[_currentCommandIndex + 1]
                                      : null;
    
    private int _currentCommandIndex = 0;
    public bool MoveNext()
    {
        if (_currentCommandIndex + 1 >= _command.Length)
        {
            return false;
        }

        _currentCommandIndex++;
        return true;
    }

    public bool HasNextCommand => _currentCommandIndex + 1 < _command.Length;
}