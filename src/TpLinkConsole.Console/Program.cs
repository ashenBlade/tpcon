using JsUtils.Implementation;
using Router.Commands;
using Router.Commands.Exceptions;
using Router.Commands.Utils;
using Router.CommandsParser.CommandLineParser;
using Router.Domain.Exceptions;
using Router.TpLink;
using Router.TpLink.TLWR741ND;


try
{
    using var client = new HttpClient();
    var parser = new FSharpCommandLineParser();
    var context = parser.ParseCommandLineContext(args);
    var factory = new TpLinkCommandFactory(new TLWR741NDTpLinkRouterFactory(), 
                                           new TLWR741NDTpLinkRouterHttpMessageSender(new HtmlScriptVariableExtractor(new HtmlScriptExtractor(), 
                                                                                                                      new ScriptVariableExtractor(new Tokenizer())), 
                                                                                      client,
                                                                                      context.RouterParameters));
    var command = factory.CreateRouterCommand(context);
    await command.ExecuteAsync();
}
catch (InvalidRouterCredentialsException)
{
    Console.WriteLine($"Invalid credentials provided");
}
catch (RouterUnreachableException)
{
    Console.WriteLine($"Could not connect to router");
}
catch (UnknownCommandLineException)
{
    Console.WriteLine("Unknown command");
}
catch (Exception ex)
{
    Console.WriteLine($"Could not execute command. Unknown error.");
    Console.WriteLine(ex.Message);
}