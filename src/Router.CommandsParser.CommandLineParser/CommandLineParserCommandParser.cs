using System.Globalization;
using System.Net;
using CommandLine;
using Router.Commands;
using Router.Commands.Commands;
using Router.CommandsParser.CommandLineParser.Options;
using Router.Domain;

namespace Router.CommandsParser.CommandLineParser;

public class CommandLineParserCommandParser : ICommandParser
{
    private Parser Parser { get; }
    private static Type[] SupportedCommands => new[] {typeof(RefreshRouterArguments)};
    
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
    
    public IRouterCommand ParseCommand(IEnumerable<string> commandLineArguments)
    {
        return Parser.ParseArguments(commandLineArguments, SupportedCommands)
                     .MapResult( obj => obj switch
                                        {
                                            RefreshRouterArguments refresh => (IRouterCommand)new RefreshRouterCommand(new RouterParameters(IPAddress.Parse(refresh.IpAddress),
                                                                                                                                            refresh.Username,
                                                                                                                                            refresh.Password)),
                                            _ => throw new Exception("Unknown command")
                                        },
                                 errs => throw new Exception("Unknown command"));
    }
}