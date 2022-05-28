namespace Router.Commands.Utils.Formatters

open Router.Commands

type TableFormatter() =
    member this.Format formattable =
        FormattingTable.formatTable formattable
        
    interface IOutputFormatter with
        member this.Format formattable = this.Format formattable

