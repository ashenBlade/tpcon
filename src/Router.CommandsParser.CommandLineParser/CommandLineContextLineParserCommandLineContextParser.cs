using System.Globalization;
using CommandLine;
using Router.Commands;
using Router.Commands.Commands;
using Router.Commands.Exceptions;
using Router.CommandsParser.CommandLineParser.Options;
using Router.Domain;

namespace Router.CommandsParser.CommandLineParser;

public class CommandLineContextLineParserCommandLineContextParser : ICommandLineContextParser
{
    private Parser Parser { get; }
    private static Type[] SupportedCommands => new[]
                                               {
                                                   typeof(RefreshRouterArguments), 
                                                   typeof(HealthCheckRouterArguments),
                                                   typeof(GetWlanStatusArguments),
                                                   typeof(SetWlanSsidRouterArguments)
                                               };
    
    public CommandLineContextLineParserCommandLineContextParser(TextWriter? output = null)
    {
        Parser = InitializeParser(output);
    }

    private static Parser InitializeParser(TextWriter? output)
    {
        var parser = new Parser(settings =>
        {
            settings.AutoHelp = true;
            settings.HelpWriter = output ?? Console.Out;
            settings.ParsingCulture = CultureInfo.InvariantCulture;
            settings.CaseSensitive = false;
        });
        return parser;
    }

    private RouterParameters GetParameters(BaseRouterArguments args) =>
        new(args.GetIpAddressParsed, args.Username, args.Password);
    
    public CommandLineContext ParseCommandLineContext(string[] args)
    {
    }
}