using System.Globalization;
using System.Net;
using CommandLine;
using Router.Commands;
using Router.Commands.Commands;
using Router.Commands.Exceptions;
using Router.CommandsParser.CommandLineParser.Options;
using Router.Domain;

namespace Router.CommandsParser.CommandLineParser;

public class CommandLineParserCommandParser : ICommandParser
{
    private Parser Parser { get; }
    private static Type[] SupportedCommands => new[] {typeof(RefreshRouterArguments), typeof(HealthCheckRouterArguments)};
    
    public CommandLineParserCommandParser(TextWriter? output = null)
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
    
    public IRouterCommand ParseCommand(string[] commandLineArguments)
    {
        return Parser.ParseArguments(commandLineArguments, SupportedCommands)
                     .MapResult(obj => obj switch
                                       {
                                           RefreshRouterArguments refresh => ( IRouterCommand ) new
                                               RefreshRouterCommand(GetParameters(refresh)),
                                           HealthCheckRouterArguments health => new HealthCheckCommand(GetParameters(health)),
                                           _ => throw new UnknownCommandException(commandLineArguments)
                                       },
                                _ => throw new UnknownCommandException(commandLineArguments));
    }
}