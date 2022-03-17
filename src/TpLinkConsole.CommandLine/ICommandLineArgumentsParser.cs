namespace TpLinkConsole.CommandLine
{
    public interface ICommandLineArgumentsParser
    {
        public ICommandLineArguments Parse(string[] args);
    }
}