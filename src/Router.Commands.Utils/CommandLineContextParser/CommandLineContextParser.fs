module Router.Commands.Utils.CommandLineContextParser.CommandLineContextParser

open System.Collections.Generic
open System.Net
open Microsoft.FSharp.Collections
open Router.Commands
open Router.Domain


type Command = string list
type CommandLineContextWithCommandsParsed = {
    Command: Command
    Arguments: string list
}
type ParseCommand = string list -> CommandLineContextWithCommandsParsed

type Arguments = Map<string, string>
type CommandLineContextWithCommandsAndArgumentsParsed = {
    Command: Command
    Arguments: Arguments
}
type ParseArguments = string list -> Arguments

type ParseCommandLineContext = string list -> CommandLineContext

let parseCommandFromCommandLineInput: ParseCommand =
    (fun list ->
            let rec parseCommandFromCommandLineInputRec (result: CommandLineContextWithCommandsParsed) =
                        match result.Arguments with
                        | [] -> result
                        | first::rest when not (first.StartsWith '-') ->
                                parseCommandFromCommandLineInputRec { result with Arguments = rest; Command = first::result.Command}
                        | _ -> result
            parseCommandFromCommandLineInputRec {
                Arguments = list
                Command = []
            }
    )
    
let normalizeArgumentName (arg: string) = arg[2..]
    
let parseArgumentsFromCommandsParsed: ParseArguments =
    (fun list ->
         let rec parseInner (left: string list) (args: Arguments) =
             match left with
             | [] -> args
             | [_] -> failwith "Argument is not provided"
             | [argument; value] -> Map.add (normalizeArgumentName argument) value args
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
        let argumentsParsed = parseArgumentsFromCommandsParsed commandParsed.Arguments
        let parameters = getRouterParameters argumentsParsed
        
        let result = CommandLineContext(commandParsed.Command |> List.toArray |> Array.rev, parameters, Dictionary(argumentsParsed))
        result)
    
    