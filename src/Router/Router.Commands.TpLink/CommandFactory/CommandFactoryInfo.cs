namespace Router.Commands.TpLink.CommandFactory;

public record struct CommandFactoryInfo(string Name, string Description)
{
    public CommandFactoryInfo()
        : this(string.Empty, string.Empty)
    {
    }
}