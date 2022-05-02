using Router.Commands;
using Router.Commands.Exceptions;
using Router.Commands.TpLink;
using Router.CommandsParser.CommandLineParser;
using Router.Domain.Exceptions;


ICommandParser parser = new CommandLineParserCommandParser(new TpLinkRouterCommandFactoryFactory(), 
                                                           Console.Out);
try
{
    var command = parser.ParseCommand(args);
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
catch (Exception ex)
{
    Console.WriteLine($"Could not execute command. Unknown error.");
    Console.WriteLine(ex.Message);
}