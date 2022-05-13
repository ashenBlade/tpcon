module Router.Commands.Utils.Formatters.StringBuilderFunctions

open System.Text

type CustomStringBuilder() =
    member this.Yield _ = StringBuilder()
    
    [<CustomOperation("string")>]
    member this.AppendString(builder: StringBuilder, append: string) =
        builder.Append append |> ignore
        builder
    
    [<CustomOperation("int")>]
    member this.AppendInt(builder: StringBuilder, int: int) =
        builder.Append int |> ignore
        builder
    
    [<CustomOperation("any")>]
    member this.AppendAny<'a>(builder: StringBuilder, any: 'a) =
        builder.Append any |> ignore
        builder
    
    [<CustomOperation("many")>]
    member this.AppendMany<'a>(builder: StringBuilder, list: 'a seq) =
        for value in list do
            builder.Append value |> ignore
        builder
        
    [<CustomOperation("build")>]    
    member this.Build (builder) = builder.ToString()
    
    member this.Return (str: string) = str
    
    member this.Zero() = System.String.Empty

let str = CustomStringBuilder()