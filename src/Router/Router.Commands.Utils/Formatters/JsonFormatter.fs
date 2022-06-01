namespace Router.Commands.Utils.Formatters

open System
open System.Text.Json
open System.Text.Json.Serialization
open Router.Commands

type JsonFormatter() =
    member this.Format (formattable: 'a) =
        JsonSerializer.Serialize<'a>(formattable, JsonSerializerOptions(JsonSerializerDefaults.Web, WriteIndented = true))
        
    interface IOutputFormatter with
        member this.Format formattable = this.Format formattable