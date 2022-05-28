using Router.Domain;

namespace Router.Commands;

public record CommandLineContext(string[] Command, RouterParameters RouterParameters, IDictionary<string, string> Arguments, OutputStyle OutputStyle)
{
    private int _currentCommandIndex = 0;
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
}