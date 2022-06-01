module Router.Commands.Utils.Formatters.Formatting

open System
open System.Collections.Generic
open System.ComponentModel
open System.Reflection

type ExtractState = obj -> KeyValuePair<string, string> seq

let typeof obj = obj.GetType()


let extractPropertiesFlagged (flags: BindingFlags list) (typ: Type) =
    typ.GetProperties(flags
                      |> List.reduce (fun c n -> c ||| n))
let extractInstanceProperties typ =
    extractPropertiesFlagged [
                BindingFlags.Instance
                BindingFlags.Public
                BindingFlags.GetProperty
            ] typ



let getPropertyName (prop: PropertyInfo): string =
    match prop.GetCustomAttribute<DisplayNameAttribute>() with
    | null -> prop.Name
    | x -> x.DisplayName
    
let isSimpleType obj = 
    let ``type`` = typeof obj
    ``type``.IsPrimitive || typedefof<String> = ``type``

let rec extractObjectPropertyState (obj: obj) (prop: PropertyInfo) =
    let propertyName = getPropertyName prop
    let value = prop.GetValue(obj)
    if isSimpleType value then
        [| KeyValuePair(propertyName, value.ToString()) |]
    else
        extractInstanceProperties (value.GetType())
        |> Array.collect (extractObjectPropertyState value)
        |> Array.map (fun p -> KeyValuePair($"{propertyName}.{p.Key}", p.Value))
        
let extractState: ExtractState =
    (fun obj ->
            typeof obj
            |> extractInstanceProperties
            |> Seq.collect (extractObjectPropertyState obj))

            
    