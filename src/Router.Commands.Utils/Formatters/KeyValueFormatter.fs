namespace Router.Commands.Utils

open System
open System.Text
open Router.Commands
open Router.Commands.Utils.Formatters
open Router.Commands.Utils.Formatters.StringBuilderFunctions

[<CLSCompliant(true)>]
type KeyValueFormatter(delimiter: string) =
     member private this._delimiter = delimiter
     
     interface IOutputFormatter with
          member this.Format(formattable) =
               let state = Formatting.extractState formattable
               let x = Seq.toList state
               str {
                    many (state
                          |> Seq.map (fun p -> $"{p.Key}{this._delimiter}{p.Value}")
                          |> Seq.reduce (fun s n -> $"{s}\n{n}"))
                    build
               }