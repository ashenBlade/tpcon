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
    var context = new RouterCommandContext(cmd.Command,
                                           cmd.Arguments,
                                           formatter,
                                           Console.Out,
                                           cmd.RouterConnectionParameters);
    var factory = new TLWR741NDTpLinkCommandFactory(cmd.RouterConnectionParameters);
    var command = factory.CreateRouterCommand(context);
    await command.ExecuteAsync();
}
catch (InvalidRouterCredentialsException)
{
    Console.WriteLine($"Не удалось подключиться к роутеру: Неправильные логин или пароль");
    Environment.ExitCode = 1;
}
catch (RouterUnreachableException)
{
    Console.WriteLine($"Не удалось подключиться к роутеру: Роутер недоступен");
    Environment.ExitCode = 2;
}
catch (UnknownCommandException unknown)
{
    Console.WriteLine($"Неизвестная команда: \"{unknown.Unknown}\"\nЧтобы увидеть список возможных команд введите '--help'");
    Environment.ExitCode = 3;
}
catch (CommandLineException cmd)
{
    Console.WriteLine(cmd.Message);
    Environment.ExitCode = 4;
}
catch (RouterException router)
{
    Console.WriteLine(router.Message);
    Environment.ExitCode = 5;
}
catch (Exception ex)
{
    Console.WriteLine("Не удалось выполнить команду: Произошла неизвестная ошибка");
    Console.Error.WriteLine(ex);
    Environment.ExitCode = 5;
}