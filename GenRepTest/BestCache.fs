namespace GenRepTest

open System.IO
open System.Collections.Generic
open NUnit.Framework
open FsUnit
open GenRepLib

module BestCache =

    let testdict = dict["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",{Best="e4";Resp="c5";Eval=60}]|>Dictionary
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let SaveWhiteUndefined() =
        (fun() -> BestCache.SaveWhite testdict) |> should throw typeof<System.Exception>
        (fun() -> BestCache.SaveWhite testdict) |> should (throwWithMessage "White Cache file not defined") typeof<System.Exception>

    [<Test>]
    let SaveWhite() =
        let testwhite = @"D:\Github\GenRep\TestData\TempBestCache.txt"
        BestCache.WhiteCache <- testwhite
        BestCache.SaveWhite testdict
        File.Exists testwhite|>should equal true
        BestCache.WhiteCache <- ""
        File.Delete testwhite

    [<Test>]
    let SaveBlackUndefined() =
        (fun() -> BestCache.SaveBlack testdict) |> should throw typeof<System.Exception>
        (fun() -> BestCache.SaveBlack testdict) |> should (throwWithMessage "Black Cache file not defined") typeof<System.Exception>

    [<Test>]
    let SaveBlack() =
        let testblack = @"D:\Github\GenRep\TestData\TempBestCache.txt"
        BestCache.BlackCache <- testblack
        BestCache.SaveBlack testdict
        File.Exists testblack|>should equal true
        BestCache.BlackCache <- ""
        File.Delete testblack

    [<Test>]
    let LoadWhiteUndefined() =
        (fun() -> BestCache.LoadWhite()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> BestCache.LoadWhite()|>ignore) |> should (throwWithMessage "White Cache file not defined") typeof<System.Exception>

    [<Test>]
    let LoadWhiteDuff() =
        let testwhite = @"D:\Github\GenRep\TestData\DuffCache.txt"
        BestCache.WhiteCache <- testwhite
        (fun() -> BestCache.LoadWhite()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> BestCache.LoadWhite()|>ignore) |> should (throwWithMessage "Invalid line in White Cache:\"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",,,[\"e4\";\"d4\"]") typeof<System.Exception>
        BestCache.WhiteCache <- ""

    [<Test>]
    let LoadWhite() =
        let testwhite = @"D:\Github\GenRep\TestData\BestCache.txt"
        BestCache.WhiteCache <- testwhite
        let ans = BestCache.LoadWhite()
        BestCache.WhiteCache <- ""
        ans.Count|> should equal 1
        let ans0 = ans["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"]
        ans0.Best|>should equal "e4"
        ans0.Resp|>should equal "c5"
        ans0.Eval|>should equal 60

    [<Test>]
    let LoadBlackUndefined() =
        (fun() -> BestCache.LoadBlack()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> BestCache.LoadBlack()|>ignore) |> should (throwWithMessage "Black Cache file not defined") typeof<System.Exception>

    [<Test>]
    let LoadBlackDuff() =
        let testblack = @"D:\Github\GenRep\TestData\DuffCache.txt"
        BestCache.BlackCache <- testblack
        (fun() -> BestCache.LoadBlack()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> BestCache.LoadBlack()|>ignore) |> should (throwWithMessage "Invalid line in Black Cache:\"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",,,[\"e4\";\"d4\"]") typeof<System.Exception>
        BestCache.BlackCache <- ""

    [<Test>]
    let LoadBlack() =
        let testblack = @"D:\Github\GenRep\TestData\BestCache.txt"
        BestCache.BlackCache <- testblack
        let ans = BestCache.LoadBlack()
        BestCache.BlackCache <- ""
        ans.Count|> should equal 1
        let ans0 = ans["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"]
        ans0.Best|>should equal "e4"
        ans0.Resp|>should equal "c5"
        ans0.Eval|>should equal 60
