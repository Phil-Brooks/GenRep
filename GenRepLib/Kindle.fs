namespace GenRepLib

open System.IO
open KindleChess

module Kindle =
    let mutable tfol = ""
    let mutable ifol = ""

    let WhiteOpf(title) =
        let pfol = Rep.wfol()
        let ofol = Path.Combine(pfol,"Kindle")
        let chs = Rep.WhiteChapters()
        let fls = 
            chs
            |>Array.map(fun ch -> Path.Combine(pfol,ch + ".pgn"))
        //TODO do something with
        Book.genh title fls pfol tfol ifol ofol

    let BlackOpf(title) =
        let pfol = Rep.bfol()
        let ofol = Path.Combine(pfol,"Kindle")
        let chs = Rep.BlackChapters()
        let fls = 
            chs
            |>Array.map(fun ch -> Path.Combine(pfol,ch + ".pgn"))
        //TODO do something with
        Book.genh title fls pfol tfol ifol ofol

    let BookOpf(title) =
        let pfol = Book.bkfol
        let ofol = Path.Combine(pfol,"Kindle")
        let fl = Path.Combine(pfol,title + ".pgn")
        //TODO do something with
        Book.genhb title fl pfol tfol ifol ofol
