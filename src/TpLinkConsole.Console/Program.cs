using Router.Commands;

ICommandParser parser = null!;
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
