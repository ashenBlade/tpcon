using System.Net;
using Router.Domain;

namespace Router.Commands.TpLink;

public class TpLinkCommandLineContextParser : ICommandLineContextParser
{
    private static bool IsArgumentName(string arg) => arg.StartsWith('-');
    public CommandContext ParseCommandContext(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);
        
        var command = new List<string>();
        var arguments = new Dictionary<string, string>();
        
        var i = 0;
        for (; i < args.Length; i++)
        {
            var verb = args[i];
            if (IsArgumentName(verb))
            {
                continue;
            }
            command.Add(verb);
        }

        for (; i < args.Length; i++)
        {
            var argument = args[i];
            if (!IsArgumentName(argument))
            {
                throw new ArgumentException($"Expected argument name. Given: {argument}");
            }

            if (i + 1 == args.Length)
            {
                throw new ArgumentException($"Argument value is not provided");
            }

            var value = args[i + 1];
            arguments[argument] = value;
            i++;
        }

        return new CommandContext(command.ToArray(), ExtractRouterParameters(arguments));
    }

    private static RouterParameters ExtractRouterParameters(Dictionary<string, string> arguments)
    {
        arguments.TryGetValue("username", out var username);
        arguments.TryGetValue("password", out var password);
        arguments.TryGetValue("address", out var address);
        var parameters = new RouterParameters(address is null
                                                  ? null
                                                  : IPAddress.Parse(address), username, password);
        return parameters;
    }
}