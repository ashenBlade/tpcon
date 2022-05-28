namespace Router.Commands.Utils.Formatters

open System.IO
open System.Xml.Serialization
open Router.Commands

type XmlFormatter() =
    interface IOutputFormatter with
        member this.Format formattable =
            let serializer = XmlSerializer (formattable.GetType())
            use stream = new MemoryStream()
            use writer = new StreamWriter(stream)
            serializer.Serialize (writer, formattable)
            use reader = new StreamReader(stream)
            stream.Position <- 0
            reader.ReadToEnd()