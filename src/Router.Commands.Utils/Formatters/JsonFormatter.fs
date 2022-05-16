namespace Router.Commands.Utils.Formatters

open System.Text.Json
open Router.Commands

type JsonFormatter() =
    interface IOutputFormatter with
        member this.Format formattable =
            JsonSerializer.Serialize(formattable, JsonSerializerOptions(JsonSerializerDefaults.Web))