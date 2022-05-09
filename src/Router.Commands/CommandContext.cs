using System.Collections;
using System.Net.Http.Headers;
using System.Text;
using Router.Domain;

namespace Router.Commands;

public record CommandContext(string[] Command, RouterParameters RouterParameters)
{
    public string CurrentCommand =>
        _currentCommandIndex < Command.Length
            ? Command[_currentCommandIndex]
            : string.Empty;

    public string NextCommand => _currentCommandIndex + 1 < Command.Length
                                      ? Command[_currentCommandIndex + 1]
                                      : string.Empty;
    
    private int _currentCommandIndex = 0;
    public bool MoveNext()
    {
        if (_currentCommandIndex + 1 >= Command.Length)
        {
            return false;
        }

        _currentCommandIndex++;
        return true;
    }

    public bool HasNextCommand => _currentCommandIndex < Command.Length;
}