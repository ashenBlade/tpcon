namespace Router.Commands.Utils

open System
open Router.Commands
open Router.Commands.CommandLine
open Router.Commands.CommandLine.Exceptions
open Router.Commands.Utils.CommandLineContextParser.CommandLineContextParser

[<CLSCompliant(true)>]
type FSharpCommandLineParser() =
    member this.ParseCommandLineContext(args: string []) : CommandLineContext =
        match parseCommandLineContext (Array.toList args) with
        | Ok context -> context
        | Error err ->
            match err with
            | ArgumentExpectedError expected -> raise (ArgumentValueExpectedException(expected, args))
            | IncorrectArgumentValueError (argument, actual) ->
                raise (IncorrectArgumentValueException(argument, actual, args))
            | DuplicatedArgumentError argument -> raise (DuplicatedArgumentsException(argument, args))

    interface ICommandLineContextParser with
        member this.ParseCommandLineContext(args) = this.ParseCommandLineContext args
