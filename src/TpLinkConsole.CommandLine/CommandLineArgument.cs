namespace TpLinkConsole.CommandLine;

public class CommandLineArgument : Argument
{
    public string Alias { get; }

    public CommandLineArgument(string name, string value, string alias) 
        : base(name, value)
    {
        Alias = alias;
    }
}