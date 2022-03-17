namespace TpLinkConsole.Console
{
    public interface ICommandLineArguments
    {
        public string? this[string argument] { get; }
        public bool TryGetArgument(string argument, out string value);
        public IEnumerable<string> GetCommand();
    }
}