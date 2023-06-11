namespace GenRepTest

open System.IO
open NUnit.Framework
open FsUnit
open GenRepLib

module Pgn =
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let LoadMissing() =
        let simple = @"D:\Github\GenRep\TestData\Missing.pgn"
        (fun() -> Pgn.Load simple|>ignore) |> should throw typeof<System.IO.FileNotFoundException>

    [<Test>]
    let LoadSimple() =
        let simple = @"D:\Github\GenRep\TestData\Simple.pgn"
        let gm = Pgn.Load simple
        gm.BlackPlayer|>should equal "Grob"
        gm.MoveText.Length|>should equal 5
        gm.MoveText.Head|>FsChessPgn.PgnWrite.MoveTextEntryStr|>should equal "1. g4"

    [<Test>]
    let LoadComplex() =
        let complex = @"D:\Github\GenRep\TestData\Complex.pgn"
        let gm = Pgn.Load complex
        gm.BlackPlayer|>should equal "Grunfeld"
        gm.MoveText.Length|>should equal 28
        gm.MoveText.Head|>FsChessPgn.PgnWrite.MoveTextEntryStr|>should equal "1. d4"

    [<Test>]
    let SaveSimple() =
        let simple = @"D:\Github\GenRep\TestData\Simple.pgn"
        let nsimple = @"D:\Github\GenRep\TestData\nSimple.pgn"
        let gm = Pgn.Load simple
        Pgn.Save nsimple gm
        let lines = File.ReadAllLines nsimple
        lines.Length|>should equal 16

    [<Test>]
    let SaveComplex() =
        let complex = @"D:\Github\GenRep\TestData\Complex.pgn"
        let ncomplex = @"D:\Github\GenRep\TestData\nComplex.pgn"
        let gm = Pgn.Load complex
        Pgn.Save ncomplex gm
        let lines = File.ReadAllLines ncomplex
        lines.Length|>should equal 26

