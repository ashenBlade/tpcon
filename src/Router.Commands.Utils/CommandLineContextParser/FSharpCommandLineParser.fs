namespace Router.Commands.Utils
open System
open Router.Commands
open Router.Commands.Utils.CommandLineContextParser

[<CLSCompliant(true)>]
type FSharpCommandLineParser() =
    member this.ParseCommandLineContext (args: string[]): CommandLineContext =
        CommandLineContextParser.parseCommandLineContext (Array.toList args)
    interface ICommandLineContextParser with
        member this.ParseCommandLineContext(args) = this.ParseCommandLineContext args
