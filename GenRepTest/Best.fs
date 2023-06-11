namespace GenRepTest

open GenRepLib
open NUnit.Framework
open FsUnit
open System.IO

module Best =
    
    [<SetUp>]
    let Setup () =
        Best.depth <- 5
        ()

    [<Test>]
    let CalcHome5() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let ans = Best.Calc fen
        ans.Best|>should equal "e4"
        ans.Resp|>should equal "c5"
        ans.Eval|>should equal 60
 
    [<Test>]
    let GetWhiteMissing() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testwhite = @"D:\Github\GenRep\TestData\TempCache.txt"
        BestCache.WhiteCache <- testwhite
        File.Exists testwhite|>should equal false
        let ans = Best.GetWhite fen
        File.Exists testwhite|>should equal true
        BestCache.WhiteCache <- ""
        File.Delete testwhite
        ans.Best|>should equal "e4"
        ans.Resp|>should equal "e5"
        ans.Eval|>should equal 39

    [<Test>]
    let GetWhite() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testwhite = @"D:\Github\GenRep\TestData\BestCache.txt"
        BestCache.WhiteCache <- testwhite
        let ans = Best.GetWhite fen
        ans.Best|>should equal "e4"
        ans.Resp|>should equal "c5"
        ans.Eval|>should equal 60

    [<Test>]
    let GetBlackMissing() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testblack = @"D:\Github\GenRep\TestData\TempCache.txt"
        BestCache.BlackCache <- testblack
        File.Exists testblack|>should equal false
        let ans = Best.GetBlack fen
        File.Exists testblack|>should equal true
        BestCache.BlackCache <- ""
        File.Delete testblack
        ans.Best|>should equal "e4"
        ans.Resp|>should equal "e5"
        ans.Eval|>should equal 39

    [<Test>]
    let GetBlack() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testblack = @"D:\Github\GenRep\TestData\BestCache.txt"
        BestCache.BlackCache <- testblack
        let ans = Best.GetBlack fen
        ans.Best|>should equal "e4"
        ans.Resp|>should equal "c5"
        ans.Eval|>should equal 60
