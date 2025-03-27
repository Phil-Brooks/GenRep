namespace GenRepLib

open System.IO
open KindleChess
open Fli

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
        Book.genh title fls pfol tfol ifol ofol

    let BlackOpf(title) =
        let pfol = Rep.bfol()
        let ofol = Path.Combine(pfol,"Kindle")
        let chs = Rep.BlackChapters()
        let fls = 
            chs
            |>Array.map(fun ch -> Path.Combine(pfol,ch + ".pgn"))
        Book.genh title fls pfol tfol ifol ofol

    let BookOpf(title) =
        let pfol = Book.bkfol
        let ofol = Path.Combine(pfol,"Kindle")
        let fl = Path.Combine(pfol,title + ".pgn")
        Book.genhb title fl pfol tfol ifol ofol
        //TODO use kindlegen to create mobi
        let book = Path.Combine(ofol,"book.opf")
        let mob = title + ".mobi"
        cli {
            Exec "kindlegen.exe" 
            Arguments [book; "-o"; mob]
        }
        |>Command.execute
        |>ignore
