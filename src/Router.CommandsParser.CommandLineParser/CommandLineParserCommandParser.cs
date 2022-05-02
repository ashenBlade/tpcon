using System.Globalization;
using CommandLine;
using Router.Commands;
using Router.Commands.Exceptions;
using Router.CommandsParser.CommandLineParser.Options;
using Router.Domain;

namespace Router.CommandsParser.CommandLineParser;

public class CommandLineParserCommandParser : ICommandParser
{
    private readonly IRouterCommandFactoryFactory _factoryFactory;
    private Parser Parser { get; }
    private static Type[] SupportedCommands => new[] {typeof(RefreshRouterArguments), typeof(HealthCheckRouterArguments)};
    
    public CommandLineParserCommandParser(IRouterCommandFactoryFactory factoryFactory, TextWriter? output = null)
    {
        ArgumentNullException.ThrowIfNull(factoryFactory);
        _factoryFactory = factoryFactory;
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

    private IRouterCommandFactory GetFactory(BaseRouterArguments arguments)
    {
        return _factoryFactory.CreateRouterCommandFactory(new RouterContext(GetParameters(arguments)));
    }
    
    private RouterParameters GetParameters(BaseRouterArguments args) =>
        new(args.GetIpAddressParsed, args.Username, args.Password);
    
    public IRouterCommand ParseCommand(string[] commandLineArguments)
    {
        return Parser.ParseArguments(commandLineArguments, SupportedCommands)
                     .MapResult(obj => obj switch
                                       {
                                           RefreshRouterArguments refresh => (IRouterCommand) GetFactory(refresh).CreateRefreshRouterCommand(),
                                           HealthCheckRouterArguments health => GetFactory(health).CreateHealthCheckCommand(),
                                           _ => throw new UnknownCommandException(commandLineArguments)
                                       },
                                _ => throw new UnknownCommandException(commandLineArguments));
    }
}