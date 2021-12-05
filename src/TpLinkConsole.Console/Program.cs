// See https://aka.ms/new-console-template for more information

using System;
using System.Net;
using System.Runtime.CompilerServices;
using TpLinkConsole.Console;
using TpLinkConsole.Console.CommandLineUtils;
using TpLinkConsole.Core;


var controller = GetRouterController(args);
Console.WriteLine($"Testing connection...");
var success = await controller.TestConnectionAsync();
var previousColor = Console.BackgroundColor;
if (success)
{
    Console.BackgroundColor = ConsoleColor.Green;
    Console.WriteLine("Connection exists");
}
else
{
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine("Connection failed");
}
Console.BackgroundColor = previousColor;



RouterController GetRouterController(string[] args)
{
    var parser = new CommandLineArgumentsParser();
    ICommandLineArguments parsed;
    try
    {
        parsed = parser.Parse(args);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
    return new RouterController(IPAddress.Parse(parsed?["address"] ?? "192.168.0.1" ),
                                parsed?["username"] ?? "admin",
                                parsed?["password"] ?? "admin");
}