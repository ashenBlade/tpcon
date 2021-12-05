using System.Collections.Generic;

namespace TpLinkConsole.Console
{
    public interface ICommandLineArgumentsParser
    {
        public ICommandLineArguments Parse(string[] args);
    }
}