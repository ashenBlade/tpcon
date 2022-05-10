using System.Text;
using Router.Domain;

namespace Router.Commands.TpLink;

internal record RouterCommandContext(TpLinkRouter Router)
{
    private readonly string[] _command;
    
    public RouterCommandContext(TpLinkRouter router, string[] command) 
    : this(router)
    {
        ArgumentNullException.ThrowIfNull(command);
        _command = command;
    }
    public string CurrentCommand =>
        _currentCommandIndex < _command.Length
            ? _command[_currentCommandIndex]
            : string.Empty;

    public string NextCommand => _currentCommandIndex + 1 < _command.Length
                                     ? _command[_currentCommandIndex + 1]
                                     : string.Empty;
    
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

    public bool HasNextCommand => _currentCommandIndex < _command.Length;
}