namespace TpLinkConsole.CommandLine
{
    public class CommandLineArgumentsParser : ICommandLineArgumentsParser
    {
        private IReadOnlyCollection<CommandLineParameter> _parameters;
        
        public CommandLineArgumentsParser(IReadOnlyCollection<CommandLineParameter> parameters)
        {
            _parameters = parameters;
        }

        public ICommandLineArguments Parse(string[] args)
        {
            throw new NotImplementedException();
        }

        private bool IsParameterName(string s)
        {
            return s.StartsWith("--");
        }
    }
}