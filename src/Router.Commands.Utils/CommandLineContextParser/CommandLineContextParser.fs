module Router.Commands.Utils.CommandLineContextParser.CommandLineContextParser

open System.Collections.Generic
open System.Net
open Microsoft.FSharp.Collections
open Router.Commands
open Router.Domain

type ParsingError =
    | ArgumentExpectedError of string



type Command = string list
type CommandLineContextWithCommandsParsed = {
    Command: Command
    Arguments: string list
}

type ParseCommand = string list -> Result<CommandLineContextWithCommandsParsed, ParsingError>

type Arguments = Map<string, string>
type CommandLineContextWithCommandsAndArgumentsParsed = {
    Command: Command
    Arguments: Arguments
}
type ParseArguments = string list -> Result<Arguments, ParsingError> 

type ParseCommandLineContext = string list -> Result<CommandLineContext, ParsingError>

let parseCommandFromCommandLineInput: ParseCommand =
    (fun list ->
            let rec parseCommandFromCommandLineInputRec (result: CommandLineContextWithCommandsParsed): Result<CommandLineContextWithCommandsParsed, ParsingError> =
                        match result.Arguments with
                        | [] -> Ok result
                        | first::rest when not (first.StartsWith '-') ->
                                parseCommandFromCommandLineInputRec { result with Arguments = rest; Command = first::result.Command}
                        | _ -> Ok result
            parseCommandFromCommandLineInputRec {
                Arguments = list
                Command = []
            }
    )
    
let normalizeArgumentName (arg: string) = arg[2..]
    
let parseArgumentsFromCommandsParsed: ParseArguments =
    (fun list ->
         let rec parseInner (left: string list) (args: Arguments): Result<Arguments,ParsingError> =
             match left with
             | [] -> Ok args
             | [arg] -> Error (ParsingError.ArgumentExpectedError arg)
             | [argument; value] -> Ok (Map.add (normalizeArgumentName argument) value args)
             | argument::value::rest -> parseInner rest (Map.add (normalizeArgumentName argument) value args)
         parseInner list Map.empty
    )

let (??>) =
    fun option fallback ->
        match option with
        | None -> fallback
        | Some x -> x

let fallback value defaultValue map =
    (Map.tryFind value map) ??> defaultValue

let getRouterParameters (args: Arguments): RouterParameters =
    let address = fallback "address" "192.168.0.1" args
    let username = fallback "username" "admin" args
    let password = fallback "password" "admin" args
    RouterParameters(IPAddress.Parse address, username, password)

let parseCommandLineContext: ParseCommandLineContext =
    (fun args ->
        let commandParsed = parseCommandFromCommandLineInput args
        match commandParsed with
        | Ok cmd -> 
                let argumentsParsed = parseArgumentsFromCommandsParsed cmd.Arguments
                match argumentsParsed with
                | Ok args -> 
                        let parameters = getRouterParameters args
                        Ok (CommandLineContext(cmd.Command |> List.toArray |> Array.rev, parameters, Dictionary(args)))
                | Error err -> Error err
        | Error err -> Error err)
    