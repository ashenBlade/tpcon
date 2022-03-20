namespace TpLinkConsole.CommandLine
{
    public class CommandLineArguments : ICommandLineArguments
    {
        private readonly Dictionary<string, Argument> _arguments;
        public IEnumerable<string> MainCommand { get; }

        public CommandLineArguments(IEnumerable<string> mainCommand, 
                                    params Argument[] arguments)
        {
            MainCommand = mainCommand;
            _arguments = arguments.ToDictionary(arg => arg.Name, arg => arg);
        }

        public Argument? this[string name] => 
            _arguments[name];

        public IEnumerable<Argument> Arguments =>
            _arguments.Select(pair => pair.Value);

        
        public bool TryGetArgument(string name, out Argument? value)
        {
            return _arguments.TryGetValue(name, out value);
        }

        public bool HasArgument(string name)
        {
            return _arguments.ContainsKey(name);
        }
    }
}