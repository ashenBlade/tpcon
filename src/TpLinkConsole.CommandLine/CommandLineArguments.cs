namespace TpLinkConsole.CommandLine
{
    public class CommandLineArguments : ICommandLineArguments
    {
        public CommandLineArgument? this[string name] =>
            throw new NotImplementedException();

        public IEnumerable<string> MainCommand { get; }
        public bool TryGetArgument(string name, out CommandLineArgument? value)
        {
            throw new NotImplementedException();
        }
    }
}