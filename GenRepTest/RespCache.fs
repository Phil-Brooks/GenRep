namespace GenRepTest

open System.IO
open System.Collections.Generic
open NUnit.Framework
open FsUnit
open GenRepLib

module RespCache =

    let testdict = dict["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",["e4";"d4"]]|>Dictionary
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let SaveWhiteUndefined() =
        RespCache.wcache <- ""
        (fun() -> RespCache.SaveWhite testdict) |> should throw typeof<System.Exception>
        (fun() -> RespCache.SaveWhite testdict) |> should (throwWithMessage "White Cache file not defined") typeof<System.Exception>

    [<Test>]
    let SaveWhite() =
        let testwhite = @"D:\Github\GenRep\TestData\TempCache.txt"
        Resp.SetupWhite testwhite
        RespCache.SaveWhite testdict
        File.Exists testwhite|>should equal true
        //RespCache.WhiteCache <- ""
        File.Delete testwhite

    [<Test>]
    let SaveBlackUndefined() =
        RespCache.bcache <- ""
        (fun() -> RespCache.SaveBlack testdict) |> should throw typeof<System.Exception>
        (fun() -> RespCache.SaveBlack testdict) |> should (throwWithMessage "Black Cache file not defined") typeof<System.Exception>

    [<Test>]
    let SaveBlack() =
        let testblack = @"D:\Github\GenRep\TestData\TempCache.txt"
        Resp.SetupBlack testblack
        RespCache.SaveBlack testdict
        File.Exists testblack|>should equal true
        //RespCache.BlackCache <- ""
        File.Delete testblack

    [<Test>]
    let LoadWhiteUndefined() =
        RespCache.wcache <- ""
        (fun() -> RespCache.LoadWhite()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> RespCache.LoadWhite()|>ignore) |> should (throwWithMessage "White Cache file not defined") typeof<System.Exception>

    [<Test>]
    let LoadWhiteDuff() =
        let testwhite = @"D:\Github\GenRep\TestData\DuffCache.txt"
        (fun() -> Resp.SetupWhite testwhite) |> should throw typeof<System.Exception>
        (fun() -> Resp.SetupWhite testwhite) |> should (throwWithMessage "Invalid line in White Cache:\"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",,,[\"e4\";\"d4\"]") typeof<System.Exception>
        //RespCache.WhiteCache <- ""

    [<Test>]
    let LoadWhite() =
        let testwhite = @"D:\Github\GenRep\TestData\Cache.txt"
        Resp.SetupWhite testwhite
        let ans = RespCache.LoadWhite()
        //RespCache.WhiteCache <- ""
        ans.Count|> should equal 1
        let ans0 = ans["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"]
        ans0.Length|>should equal 2
        ans0[1]|>should equal "d4"

    [<Test>]
    let LoadBlackUndefined() =
        RespCache.bcache <- ""
        (fun() -> RespCache.LoadBlack()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> RespCache.LoadBlack()|>ignore) |> should (throwWithMessage "Black Cache file not defined") typeof<System.Exception>

    [<Test>]
    let LoadBlackDuff() =
        let testblack = @"D:\Github\GenRep\TestData\DuffCache.txt"
        (fun() -> Resp.SetupBlack testblack) |> should throw typeof<System.Exception>
        (fun() -> Resp.SetupBlack testblack) |> should (throwWithMessage "Invalid line in Black Cache:\"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",,,[\"e4\";\"d4\"]") typeof<System.Exception>
        //RespCache.BlackCache <- ""

    [<Test>]
    let LoadBlack() =
        let testblack = @"D:\Github\GenRep\TestData\Cache.txt"
        Resp.SetupBlack testblack
        let ans = RespCache.LoadBlack()
        //RespCache.BlackCache <- ""
        ans.Count|> should equal 1
        let ans0 = ans["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"]
        ans0.Length|>should equal 2
        ans0[1]|>should equal "d4"