namespace GenRepLib

open System.IO
open KindleChess

module Kindle =
    let mutable tfol = ""
    let mutable ofol = ""
    let WhiteOpf() =
        let chs = Rep.WhiteChapters()
        //TODO do something with
        Book.genh chs[0] tfol (Rep.wfol()) ofol
        ()
