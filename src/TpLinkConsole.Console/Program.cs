using Router.Commands;
using Router.CommandsParser.CommandLineParser;

ICommandParser parser = new CommandLineParserCommandParser(Console.Out);
var command = parser.ParseCommand(args);
try
{
    await command.ExecuteAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Could not execute command. Unknown error.");
    Console.WriteLine(ex.Message);
}
