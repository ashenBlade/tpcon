using Router.Commands;
using Router.Commands.CommandLine.Exceptions;
using Router.Commands.Utils;
using Router.Exceptions;
using Router.Commands.TpLink.TLWR741ND;
using TpLinkConsole.Console.NETCore6;

try
{
    var parser = new FSharpCommandLineParser();
    var cmd = parser.ParseCommandLineContext(args);
    var formatter = cmd.GetOutputFormatter();
    var context = new RouterCommandContext(cmd.Command, cmd.Arguments, formatter, Console.Out, cmd.RouterConnectionParameters);
    var factory = new TLWR741NDTpLinkCommandFactory(cmd.RouterConnectionParameters);
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
catch (UnknownCommandException)
{
    Console.WriteLine("Unknown command");
}
catch (RouterException)
{
    Console.WriteLine($"Something went wrong");
}