namespace TpLinkConsole.CommandLine
{
    public class CommandLineArguments : ICommandLineArguments
    {
        private readonly Dictionary<string, string> _arguments;
        private List<string> _command;

        public CommandLineArguments(Dictionary<string, string> arguments)
        {
            _arguments = arguments;
            _command = new List<string>();
        }

        public string? this[string argument] => _arguments.TryGetValue(argument, out var value)
                                                    ? value
                                                    : null;

        public bool TryGetArgument(string argument, out string value)
        {
            return _arguments.TryGetValue(argument, out value);
        }

        public IEnumerable<string> GetCommand()
        {
            return _command;
        }
    }
}