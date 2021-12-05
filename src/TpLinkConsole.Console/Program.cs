// See https://aka.ms/new-console-template for more information

using System;
using System.Net;

var controller = new TpLinkConsole.Core.RouterController(IPAddress.Parse("192.168.0.1"), "admin", "admin");
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