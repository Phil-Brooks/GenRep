namespace GenRepTest

open System.IO
open NUnit.Framework
open FsUnit
open GenRepLib

module Resp =

    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let LiGetInitial() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let ans = Resp.LiGet fen
        ans.[0] |> should equal "e4"
        ans.Length|> should equal 12

    [<Test>]
    let LiGetFrench() =
        let fen = "rnbqkbnr/ppp2ppp/4p3/3p4/3PP3/8/PPP2PPP/RNBQKBNR w KQkq - 0 3"
        let ans = Resp.LiGet fen
        ans.[0] |> should equal "Nc3"
        ans.Length|> should equal 12

    [<Test>]
    let LiGetNone() =
        let fen = "rnbqkbnr/ppp1pppp/8/3p4/1P4P1/8/P1PPPP1P/RNBQKBNR b KQkq - 0 2"
        let ans = Resp.LiGet fen
        ans.Length|> should equal 0

    [<Test>]
    let GetWhiteMissing() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testwhite = @"D:\Github\GenRep\TestData\TempCache.txt"
        RespCache.WhiteCache <- testwhite
        File.Exists testwhite|>should equal false
        let ans = Resp.GetWhite(fen)
        File.Exists testwhite|>should equal true
        RespCache.WhiteCache <- ""
        File.Delete testwhite
        ans.[0] |> should equal "e4"
        ans.Length|> should equal 10

    [<Test>]
    let GetWhite() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testwhite = @"D:\Github\GenRep\TestData\Cache.txt"
        RespCache.WhiteCache <- testwhite
        let ans = Resp.GetWhite(fen)
        ans.[0] |> should equal "e4"
        ans.Length|> should equal 2

    [<Test>]
    let GetBlackMissing() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testblack = @"D:\Github\GenRep\TestData\TempCache.txt"
        RespCache.BlackCache <- testblack
        File.Exists testblack|>should equal false
        let ans = Resp.GetBlack(fen)
        File.Exists testblack|>should equal true
        RespCache.BlackCache <- ""
        File.Delete testblack
        ans.[0] |> should equal "e4"
        ans.Length|> should equal 10

    [<Test>]
    let GetBlack() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let testblack = @"D:\Github\GenRep\TestData\Cache.txt"
        RespCache.BlackCache <- testblack
        let ans = Resp.GetBlack(fen)
        ans.[0] |> should equal "e4"
        ans.Length|> should equal 2
