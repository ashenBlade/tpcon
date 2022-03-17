namespace TpLinkConsole.CommandLine;

public class CommandLineParameter
{
    public string Name { get; }
    public IReadOnlyList<string> Aliases { get; }
    
    public CommandLineParameter(string name, params string[] aliases)
    {
        Name = name;
        Aliases = aliases.ToList();
    }
}