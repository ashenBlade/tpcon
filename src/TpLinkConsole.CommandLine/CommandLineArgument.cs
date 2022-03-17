namespace TpLinkConsole.CommandLine;

public class CommandLineArgument : Argument
{
    public IReadOnlyList<string> Aliases { get; }

    public CommandLineArgument(string name, string value, params string[] aliases) 
        : base(name, value)
    {
        Aliases = aliases.ToList();
    }
}