using Router.Commands;
using Router.Commands.Exceptions;
using Router.Commands.Utils;

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
                   _ => throw new ArgumentNotSupportedException(context.Command, "Only plain style supported for now"),
               };
    }
}