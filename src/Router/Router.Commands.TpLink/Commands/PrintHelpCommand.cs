namespace Router.Commands.TpLink.Commands;

public class PrintHelpCommand : TpLinkCommand
{
    private readonly TextWriter _writer;
    private readonly string _command;
    private readonly string _description;

    public PrintHelpCommand(TextWriter writer, string command, string description)
    {
        _writer = writer;
        _command = command;
        _description = description;
    }

    private static string Tabulate(string str, int tabularCount = 1)
    {
        if (!str.StartsWith('\n'))
        {
            str = str.Insert(0, "\n");
        }

        return str.Replace("\n", $"\n{new string('\t', tabularCount)}");
    }

    private static string FormatHelpMessageSection(string name, string content) =>
        $"{name}:{Tabulate(content)}\n";


    private string GetFormattedHelpMessage => $"{CommandSection}{DescriptionSection}";

    private string CommandSection =>
        FormatHelpMessageSection("Command", _command);

    private string DescriptionSection =>
        FormatHelpMessageSection("Description", _description);


    public override async Task ExecuteAsync()
    {
        await _writer.WriteLineAsync(GetFormattedHelpMessage);
    }
}