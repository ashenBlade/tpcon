namespace JsUtils.Parser.FSharp

open JsUtils

type FSharpParser() =
    interface IJsVariableExtractor with
        member this.ExtractVariables (script) =
            failwith "todo"