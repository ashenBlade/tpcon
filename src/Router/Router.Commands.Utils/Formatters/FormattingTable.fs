module Router.Commands.Utils.Formatters.FormattingTable

open System
open System.Collections.Generic
open System.Text
open Microsoft.FSharp.Core

type ColumnName = string
type Value = string

type TableRow = KeyValuePair<ColumnName, Value> list

let getMaxWidth (row: TableRow): int =
    let maxLen (pair: KeyValuePair<ColumnName, Value>): int =
        Math.Max(pair.Key.Length, pair.Value.Length)
    let rec getMaxWidthInner (row: TableRow) (max: int): int =
        match row with
        | [] -> max
        | [pair] -> Math.Max(maxLen pair, max)
        | pair::rest -> getMaxWidthInner rest (Math.Max(maxLen pair, max))
    getMaxWidthInner row 0

let fillLength (len: int) (str: string): string =
    str.PadRight len

let fillLengthPair (max: int) (pair: KeyValuePair<ColumnName, Value>): KeyValuePair<ColumnName, Value> =
    KeyValuePair(fillLength max pair.Key, fillLength max pair.Value)

let createTable (pairs: KeyValuePair<ColumnName, Value> list): string =
    let headers = pairs
                  |> List.map (fun p -> p.Key)
                  |> String.concat "\t"
    let values = pairs
                 |> List.map (fun p -> p.Value)
                 |> String.concat "\t"
    $"%s{headers}\n%s{values}"
    

let formatTable (formattable: 'a): string =
    let pairs = Formatting.extractState formattable |> Seq.toList
    let maxWidth = getMaxWidth pairs
    pairs
    |> List.map (fillLengthPair maxWidth)
    |> createTable 