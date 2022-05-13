module Router.Commands.Utils.Formatters.Formatting

open System
open System.Collections.Generic
open System.Reflection

type ExtractState = obj -> KeyValuePair<string, string> seq

let typeof obj = obj.GetType()


let extractPropertiesFlagged (flags: BindingFlags list) (typ: Type) =
    typ.GetProperties(flags
                      |> List.reduce (fun c n -> c &&& n))
let extractProperties typ =
    extractPropertiesFlagged [] typ

let objectPropertyState (obj: obj) (prop: PropertyInfo) =
    KeyValuePair(prop.Name, prop.GetValue(obj).ToString()) 

let extractState: ExtractState =
    (fun obj ->
            typeof obj
            |> extractPropertiesFlagged [
                BindingFlags.Instance
                BindingFlags.Public
                BindingFlags.GetProperty
            ]
            |> Seq.map (objectPropertyState obj))

            
    