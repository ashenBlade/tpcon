using JsUtils.Implementation;
using Router.Commands.Exceptions;
using Router.Commands.Utils;
using Router.Domain.Exceptions;
using Router.TpLink;
using Router.TpLink.TLWR741ND;
using TpLinkConsole.Infrastructure;


using var client = new HttpClient();
var parser = new FSharpCommandLineParser();
var factory = new TLWR741NDTpLinkCommandFactory();
var application = new RouterApplication(parser, factory);
try
{
    await application.RunAsync(args);
}
catch (InvalidRouterCredentialsException)
{
    Console.WriteLine($"Invalid credentials provided");
}
catch (RouterUnreachableException)
{
    Console.WriteLine($"Could not connect to router");
}
catch (UnknownCommandException)
{
    Console.WriteLine("Unknown command");
}