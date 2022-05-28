namespace Router.Commands.Utils.Formatters

open System.Text.Json
open Router.Commands

type JsonFormatter() =
    member this.Format formattable =
        JsonSerializer.Serialize(formattable, JsonSerializerOptions(JsonSerializerDefaults.Web))
    interface IOutputFormatter with
        member this.Format formattable = this.Format formattable