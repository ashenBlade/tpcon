namespace TpLinkConsole.CommandLine
{
    public class CommandLineArguments : ICommandLineArguments
    {
        private readonly Dictionary<string, CommandLineArgument> _arguments;
        public IEnumerable<string> MainCommand { get; }

        public CommandLineArguments(IEnumerable<string> mainCommand, 
                                    params CommandLineArgument[] arguments)
        {
            MainCommand = mainCommand;
            _arguments = arguments.ToDictionary(arg => arg.Name, arg => arg);
        }

        public CommandLineArguments(IEnumerable<string> mainCommand, 
                                    Dictionary<string, CommandLineArgument> arguments)
        {
            MainCommand = mainCommand;
            _arguments = arguments;
        }

        public CommandLineArgument? this[string name] => 
            _arguments[name];

        public IEnumerable<CommandLineArgument> Arguments =>
            _arguments.Select(pair => pair.Value);

        
        public bool TryGetArgument(string name, out CommandLineArgument? value)
        {
            return _arguments.TryGetValue(name, out value);
        }
    }
}