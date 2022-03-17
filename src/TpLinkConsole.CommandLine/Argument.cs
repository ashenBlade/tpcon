namespace TpLinkConsole.CommandLine;

public class Argument
{
    public string Name { get; }
    public string Value { get; }
    public Argument(string name, string value)
    {
        Name = name;
        Value = value;
    }
}