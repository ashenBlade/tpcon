using Router.Commands;
using Router.Commands.Exceptions;
using Router.Commands.Utils.Formatters;

namespace Router.TpLink;

public static class CommandLineContextExtensions
{
    public static IOutputFormatter GetOutputFormatter(this CommandLineContext context)
    {
        return context.OutputStyle switch
               {
                   OutputStyle.KeyValue => new KeyValueFormatter(context.Arguments.TryGetValue("delimiter",
                                                                                               out var delimiter)
                                                                     ? delimiter
                                                                     : ": "),
                   OutputStyle.Json => new JsonFormatter(),
                   OutputStyle.Xml => new XmlFormatter(),
                   OutputStyle.Table => new TableFormatter(),
                   _ => throw new ArgumentNotSupportedException(context.Command, "Unknown output enum style"),
               };
    }
}