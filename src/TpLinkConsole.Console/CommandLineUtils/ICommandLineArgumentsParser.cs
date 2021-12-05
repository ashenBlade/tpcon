namespace TpLinkConsole.Console.CommandLineUtils
{
    public interface ICommandLineArgumentsParser
    {
        public ICommandLineArguments Parse(string[] args);
    }
}