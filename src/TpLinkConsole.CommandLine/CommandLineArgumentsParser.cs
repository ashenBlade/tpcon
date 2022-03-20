using TpLinkConsole.CommandLine.Exceptions;

namespace TpLinkConsole.CommandLine
{
    public class CommandLineArgumentsParser : ICommandLineArgumentsParser
    {
        public CommandLineArgumentsParser()
        { }

        public ICommandLineArguments Parse(string[] args)
        {
            var command = new List<string>();
            var arguments = new List<Argument>();
            var i = 0;
            for (; i < args.Length; i++)
            {
                if (IsParameter( args[i] ))
                {
                    break;
                }
                command.Add(args[i]);
            }
            
            for (; i < args.Length; i++)
            {
                if (!IsParameter(args[i]))
                {
                    throw new ArgumentExpectedException();
                }

                var parameter = args[i];
                i++;
                if (i == args.Length)
                {
                    throw new ArgumentValueExpectedException(parameter);
                }

                var value = args[i];
                arguments.Add(new Argument(parameter, value));
            }
            return new CommandLineArguments(command, arguments.ToArray());
        }

        private static bool IsParameter(string input)
        {
            return input.StartsWith("--");
        }
    }
}