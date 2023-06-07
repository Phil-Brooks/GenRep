namespace GenRepTest

open GenRepLib
open NUnit.Framework
open FsUnit

module Best =
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let GetHome5() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let ans = Best.Get 5 fen
        ans.Best|>should equal "g4"
        ans.Resp|>should equal "Na6"
        ans.Eval|>should equal -35
    