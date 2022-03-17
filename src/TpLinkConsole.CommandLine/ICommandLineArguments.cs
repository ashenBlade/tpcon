namespace TpLinkConsole.CommandLine
{
    public interface ICommandLineArguments
    {
        public CommandLineArgument? this[string name] { get; }
        public IEnumerable<string> MainCommand { get; }
        public bool TryGetArgument(string name, out CommandLineArgument? value);
    }
}