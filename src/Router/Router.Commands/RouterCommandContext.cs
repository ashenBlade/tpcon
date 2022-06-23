using Router.Domain;

namespace Router.Commands;

public class RouterCommandContext
{
    public IEnumerable<string> Command => _command;
    private readonly string[] _command;

    public RouterCommandContext(string[] command,
                                IDictionary<string, string> arguments,
                                IOutputFormatter outputFormatter,
                                TextWriter outputWriter,
                                RouterConnectionParameters connection)
    {
        _command = command.ToArray();
        Arguments = arguments;
        OutputFormatter = outputFormatter;
        OutputWriter = outputWriter;
        Connection = connection;
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
            _currentCommandIndex = _command.Length;
            return false;
        }

        _currentCommandIndex++;
        return true;
    }

    public bool HasNextCommand => _currentCommandIndex + 1 < _command.Length;
    public IDictionary<string, string> Arguments { get; init; }
    public IOutputFormatter OutputFormatter { get; init; }
    public TextWriter OutputWriter { get; init; }
    public RouterConnectionParameters Connection { get; }
}