namespace TpLinkConsole.CommandLine
{
    public interface ICommandLineArguments
    {
        public Argument? this[string name] { get; }
        public IEnumerable<Argument> Arguments { get; }
        public IEnumerable<string> MainCommand { get; }
        public bool TryGetArgument(string name, out Argument? value);
    }
}