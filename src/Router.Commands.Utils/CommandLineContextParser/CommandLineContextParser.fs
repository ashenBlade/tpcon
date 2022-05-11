module Router.Commands.Utils.CommandLineContextParser.CommandLineContextParser

open System.Net
open Microsoft.FSharp.Collections
open Router.Commands
open Router.Domain

type ParsingError =
    | ArgumentExpectedError of Argument: string
    | IncorrectArgumentValueError of Argument: string * Actual: string
    | DuplicatedArgumentError of Argument: string

type Arguments = Map<string, string>
type Command = string list

type CommandLineContextUnparsed =
    { Command: Command
      RouterParameters: RouterParameters
      Arguments: Arguments
      Rest: string list }

type ParsingPipe = CommandLineContextUnparsed -> Result<CommandLineContextUnparsed, ParsingError>

let (>=>) func1 func2 x =
    match (func1 x) with
    | Ok s -> func2 s
    | Error err -> Error err



type ParseCommand = ParsingPipe
type ParseArguments = ParsingPipe

type ParseCommandLineContext = string list -> Result<CommandLineContext, ParsingError>

let parseCommandFromCommandLineInput: ParseCommand =
    (fun context ->
        let rec parseCommandFromCommandLineInputRec
            (result: CommandLineContextUnparsed)
            : Result<CommandLineContextUnparsed, ParsingError> =
            match result.Rest with
            | [] -> Ok result
            | first :: rest when not (first.StartsWith '-') ->
                parseCommandFromCommandLineInputRec
                    { result with
                        Rest = rest
                        Command = first :: result.Command }
            | _ -> Ok result

        parseCommandFromCommandLineInputRec context)

let normalizeArgumentName (arg: string) = arg[2..]

let parseArgumentsFromCommandsParsed: ParseArguments =
    (fun context ->
        let rec parseInner (ctx: CommandLineContextUnparsed) : Result<CommandLineContextUnparsed, ParsingError> =
            match ctx.Rest with
            | [] -> Ok ctx
            | [ arg ] -> Error(ParsingError.ArgumentExpectedError arg)
            | argument :: value :: rest ->
                let normalized = normalizeArgumentName argument
                match Map.containsKey normalized ctx.Arguments with
                | true ->
                    Error(ParsingError.DuplicatedArgumentError normalized)
                | false ->
                    parseInner { ctx with Arguments = (Map.add normalized value ctx.Arguments)
                                          Rest = rest }

        parseInner context)

let (??>) =
    fun option fallback ->
        match option with
        | None -> fallback
        | Some x -> x

let fallback value defaultValue map =
    (Map.tryFind value map) ??> defaultValue

let extractRouterParameters: ParsingPipe =
    (fun ctx ->
        let args = ctx.Arguments

        let address = fallback "address" "192.168.0.1" args

        let username = fallback "username" "admin" args

        let password = fallback "password" "admin" args

        match IPAddress.TryParse address with
        | (true, ip) -> Ok { ctx with RouterParameters = RouterParameters(ip, username, password) }
        | _ -> Error(ParsingError.IncorrectArgumentValueError("address", address)))


let toUnparsedFromList (list: string list) : Result<CommandLineContextUnparsed, ParsingError> =
    Ok
        { Rest = list
          Command = List.empty
          Arguments = Map.empty
          RouterParameters = RouterParameters.Default }

let toCommandLineContext (unparsed: CommandLineContextUnparsed) : Result<CommandLineContext, ParsingError> =
    Ok(CommandLineContext(unparsed.Command |> List.rev |> List.toArray, unparsed.RouterParameters, unparsed.Arguments))

let parsingPipeline =
    parseCommandFromCommandLineInput
    >=> parseArgumentsFromCommandsParsed
    >=> extractRouterParameters


let parseCommandLineContext: ParseCommandLineContext =
    toUnparsedFromList
    >=> parsingPipeline
    >=> toCommandLineContext
