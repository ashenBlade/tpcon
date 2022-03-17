using System;
using System.Collections;
using System.Collections.Generic;

namespace TpLinkConsole.Console.CommandLineUtils
{
    public class CommandLineArgumentsParser : ICommandLineArgumentsParser
    {
        public ICommandLineArguments Parse(string[] args)
        {
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i += 2)
            {
                var arg = args[i];
                if (!IsParameterName(arg))
                {
                    throw new ArgumentException($"Parameter name expected. Got: {arg}");
                }

                string? value = null;

                if (i + 1 >= args.Length || IsParameterName(value = args[i + 1]))
                {
                    throw new ArgumentException($"Argument expected before {arg}. Got: {value}");
                }

                dict[arg] = value;
            }

            return new CommandLineArguments(dict);
        }

        private bool IsParameterName(string s)
        {
            return s.StartsWith("--");
        }
    }
}