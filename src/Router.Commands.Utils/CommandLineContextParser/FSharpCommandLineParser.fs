namespace Router.Commands.Utils
open System
open Router.Commands
open Router.Commands.Exceptions
open Router.Commands.Utils.CommandLineContextParser.CommandLineContextParser

[<CLSCompliant(true)>]
type FSharpCommandLineParser() =
    member this.ParseCommandLineContext (args: string[]): CommandLineContext =
        match parseCommandLineContext (Array.toList args) with
        | Ok context -> context
        | Error err ->
            match err with
            | ArgumentExpectedError s -> raise (ArgumentValueExpectedException(s, args))
            
    interface ICommandLineContextParser with
        member this.ParseCommandLineContext(args) = this.ParseCommandLineContext args
