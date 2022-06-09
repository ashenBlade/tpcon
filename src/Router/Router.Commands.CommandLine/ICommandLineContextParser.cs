namespace Router.Commands.CommandLine;

public interface ICommandLineContextParser
{
    public CommandLineContext ParseCommandLineContext(string[] args);
}