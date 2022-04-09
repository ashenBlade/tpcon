using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Net;
using CommandLine;
using Router.Commands.Implementation.Commands;
using Router.Commands.Implementation.Options;
using Router.Domain;
using RouterParameters = Router.Domain.RouterParameters;

namespace Router.Commands.Implementation;

public class ThirdPartyCommandParser : ICommandParser
{
    private Parser Parser { get; }
    private Type[] SupportedCommands { get; }
    
    public ThirdPartyCommandParser(TextWriter? output = null, params Type[] supportedCommands)
    {
        SupportedCommands = supportedCommands;
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
    
    public IRouterCommand ParseCommand(string[] commandLineArguments)
    {
        return Parser.ParseArguments(commandLineArguments, SupportedCommands).MapResult<Refresh, IRouterCommand>(refresh => new RefreshRouterCommand(new RouterParameters()
                                                                                                                                                     {
                                                                                                                                                         Address = IPAddress.Parse(refresh.IpAddress),
                                                                                                                                                         Password = refresh.Password,
                                                                                                                                                         Username = refresh.Username
                                                                                                                                                     }),
                                                                                                                 errs => new PrintErrorCommand("Unknown command"));
    }
}