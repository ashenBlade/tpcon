module Router.Commands.Utils.CommandLineContextParser.CommandLineContextParser

open System.Net
open Microsoft.FSharp.Collections
open Router.Commands
open Router.Commands.CommandLine
open Router.Domain

type ParsingError =
    | ArgumentExpectedError of Argument: string
    | IncorrectArgumentValueError of Argument: string * Actual: string
    | DuplicatedArgumentError of Argument: string

type Arguments = Map<string, string>
type Command = string list

type CommandLineContextUnparsed =
    { Command: Command
      RouterParameters: RouterConnectionParameters
      Arguments: Arguments
      Output: OutputStyle
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

let isArgument (arg: string) : bool = arg.StartsWith '-'

let normalizeArgumentName (arg: string) = arg[2..]

let parseArgumentsFromCommandsParsed: ParseArguments =
    (fun context ->
        let rec parseInner (ctx: CommandLineContextUnparsed) =
            match ctx.Rest with
            | [] -> Ok ctx
            | [ arg ] ->
                Ok
                    { ctx with
                        Arguments = (Map.add (normalizeArgumentName arg) null ctx.Arguments)
                        Rest = list.Empty }
            | argument :: value :: rest ->
                let normalized =
                    normalizeArgumentName argument

                match Map.containsKey normalized ctx.Arguments with
                | true -> Error(ParsingError.DuplicatedArgumentError normalized)
                | false ->
                    if isArgument value then
                        parseInner
                            { ctx with
                                Arguments = (Map.add normalized null ctx.Arguments)
                                Rest = rest @ [ value ] }
                    else
                        parseInner
                            { ctx with
                                Arguments = (Map.add normalized value ctx.Arguments)
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

        let address =
            fallback "router-address" "192.168.0.1" args

        let username =
            fallback "router-username" "admin" args

        let password =
            fallback "router-password" "admin" args

        match IPAddress.TryParse address with
        | (true, ip) -> Ok { ctx with RouterParameters = RouterConnectionParameters(ip, username, password) }
        | _ -> Error(ParsingError.IncorrectArgumentValueError("address", address)))


let toUnparsedFromList (list: string list) : Result<CommandLineContextUnparsed, ParsingError> =
    Ok
        { Rest = list
          Command = List.empty
          Arguments = Map.empty
          RouterParameters = RouterConnectionParameters.Default
          Output = OutputStyle.KeyValue }

let toCommandLineContext (unparsed: CommandLineContextUnparsed) : Result<CommandLineContext, ParsingError> =
    Ok(
        CommandLineContext(
            unparsed.Command |> List.rev |> List.toArray,
            unparsed.RouterParameters,
            unparsed.Arguments,
            unparsed.Output
        )
    )

let outputArgumentName = "output"

let (|Json|Xml|KeyValue|Table|Invalid|) str =
    match str with
    | "json" -> Json
    | "xml" -> Xml
    | "plain" -> KeyValue
    | "table" -> Table
    | _ -> Invalid

let toOutputStyle (outputString: string) : Result<OutputStyle, ParsingError> =
    match outputString with
    | Json -> Ok OutputStyle.Json
    | Xml -> Ok OutputStyle.Xml
    | KeyValue -> Ok OutputStyle.KeyValue
    | Table -> Ok OutputStyle.Table
    | _ -> Error(ParsingError.IncorrectArgumentValueError(outputArgumentName, outputString))

let extractOutputStyle: ParsingPipe =
    (fun context ->
        match Map.tryFind outputArgumentName context.Arguments with
        | Some outputString ->
            match toOutputStyle outputString with
            | Ok output -> Ok { context with Output = output }
            | Error parsingError -> Error parsingError
        | None -> Ok context

        )

let parsingPipeline =
    parseCommandFromCommandLineInput
    >=> parseArgumentsFromCommandsParsed
    >=> extractRouterParameters
    >=> extractOutputStyle


let parseCommandLineContext: ParseCommandLineContext =
    toUnparsedFromList
    >=> parsingPipeline
    >=> toCommandLineContext
