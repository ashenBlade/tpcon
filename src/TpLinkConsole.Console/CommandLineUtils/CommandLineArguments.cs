using System.Collections.Generic;
using System.Security.AccessControl;

namespace TpLinkConsole.Console
{
    public class CommandLineArguments : ICommandLineArguments
    {
        private readonly Dictionary<string, string> _arguments;

        public CommandLineArguments(Dictionary<string, string> arguments)
        {
            _arguments = arguments;
        }

        public string this[string argument] => _arguments[argument];

        public bool TryGetArgument(string argument, out string value)
        {
            return _arguments.TryGetValue(argument, out value);
        }
    }
}