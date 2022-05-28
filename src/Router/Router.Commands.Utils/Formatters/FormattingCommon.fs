module Router.Commands.Utils.Formatters.FormattingCommon

open System
open System.Collections.Generic
open System.ComponentModel
open System.Reflection

type ExtractState = obj -> KeyValuePair<string, string> seq

let typeof obj = obj.GetType()


let extractPropertiesFlagged (flags: BindingFlags list) (typ: Type) =
    typ.GetProperties(flags
                      |> List.reduce (fun c n -> c ||| n))
let extractProperties typ =
    extractPropertiesFlagged List.empty typ

let getPropertyName (prop: PropertyInfo): string =
    match prop.GetCustomAttribute<DisplayNameAttribute>() with
    | null -> prop.Name
    | x -> x.DisplayName
    
let objectPropertyState (obj: obj) (prop: PropertyInfo) =
    KeyValuePair(getPropertyName prop, prop.GetValue(obj)
                                           .ToString()) 

let extractState: ExtractState =
    (fun obj ->
            typeof obj
            |> extractPropertiesFlagged [
                BindingFlags.Instance
                BindingFlags.Public
                BindingFlags.GetProperty
            ]
            |> Seq.map (objectPropertyState obj))

            
    