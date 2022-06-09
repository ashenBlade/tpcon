using Router.Commands;
using Router.Commands.CommandLine;
using Router.Commands.Utils.Formatters;

namespace TpLinkConsole.Console.NETCore6;

public static class CommandLineContextExtensions
{
    public static IOutputFormatter GetOutputFormatter(this CommandLineContext context) =>
        context.OutputStyle switch
        {
            OutputStyle.Json  => new JsonFormatter(),
            OutputStyle.Table => new TableFormatter(),
            OutputStyle.KeyValue => new KeyValueFormatter(context.Arguments.TryGetValue("delimiter", out var delimiter)
                                                              ? delimiter
                                                              : ": "),
            OutputStyle.Xml => new XmlFormatter(),
            _ => throw new ArgumentOutOfRangeException(nameof(context.OutputStyle), $"Unknown output style: {context.OutputStyle}")
        };
}