namespace GenRepTest

open GenRepLib
open NUnit.Framework
open FsUnit
open System.IO

module BestCdb =
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let CalcHome5() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let ans = BestCdb.CdbGet fen
        ans.Best|>should equal "d2d4"
        ans.Score|>should equal 1
        ans.Rank|>should equal 2
 
    [<Test>]
    let GetWhiteMissing() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testwhite = @"D:\Github\GenRep\TestData\TempCache.txt"
        BestCdb.SetupWhite testwhite
        File.Exists testwhite|>should equal false
        let ans = BestCdb.GetWhite fen
        File.Exists testwhite|>should equal true
        //BestCache.WhiteCache <- ""
        File.Delete testwhite
        ans.Best|>should equal "d2d4"
        ans.Score|>should equal 1
        ans.Rank|>should equal 2

    [<Test>]
    let GetWhite() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testwhite = @"D:\Github\GenRep\TestData\BestCdbCache.txt"
        BestCdb.SetupWhite testwhite
        let ans = BestCdb.GetWhite fen
        ans.Best|>should equal "d2d4"
        ans.Score|>should equal 1
        ans.Rank|>should equal 2

    [<Test>]
    let GetBlackMissing() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testblack = @"D:\Github\GenRep\TestData\TempCache.txt"
        BestCdb.SetupBlack testblack
        File.Exists testblack|>should equal false
        let ans = BestCdb.GetBlack fen
        File.Exists testblack|>should equal true
        //BestCache.BlackCache <- ""
        File.Delete testblack
        ans.Best|>should equal "d2d4"
        ans.Score|>should equal 1
        ans.Rank|>should equal 2

    [<Test>]
    let GetBlack() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testblack = @"D:\Github\GenRep\TestData\BestCdbCache.txt"
        BestCdb.SetupBlack testblack
        let ans = BestCdb.GetBlack fen
        ans.Best|>should equal "d2d4"
        ans.Score|>should equal 1
        ans.Rank|>should equal 2
