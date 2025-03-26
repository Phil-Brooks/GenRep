namespace GenRepTest

open System.IO
open System.Collections.Generic
open NUnit.Framework
open FsUnit
open GenRepLib

module BestCdbCache =

    let testdict = dict["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",{Best="e4";Score=1;Rank=2}]|>Dictionary
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let SaveWhiteUndefined() =
        BestCdbCache.wcache <- ""
        (fun() -> BestCdbCache.SaveWhite testdict) |> should throw typeof<System.Exception>
        (fun() -> BestCdbCache.SaveWhite testdict) |> should (throwWithMessage "White Cache file not defined") typeof<System.Exception>

    [<Test>]
    let SaveWhite() =
        let testwhite = @"D:\Github\GenRep\TestData\TempBestCache.txt"
        BestCdb.SetupWhite testwhite
        BestCdbCache.SaveWhite testdict
        File.Exists testwhite|>should equal true
        //BestCache.WhiteCache <- ""
        File.Delete testwhite

    [<Test>]
    let SaveBlackUndefined() =
        BestCdbCache.bcache <- ""
        (fun() -> BestCdbCache.SaveBlack testdict) |> should throw typeof<System.Exception>
        (fun() -> BestCdbCache.SaveBlack testdict) |> should (throwWithMessage "Black Cache file not defined") typeof<System.Exception>

    [<Test>]
    let SaveBlack() =
        let testblack = @"D:\Github\GenRep\TestData\TempBestCache.txt"
        BestCdb.SetupBlack testblack
        BestCdbCache.SaveBlack testdict
        File.Exists testblack|>should equal true
        //BestCache.BlackCache <- ""
        File.Delete testblack

    [<Test>]
    let LoadWhiteUndefined() =
        BestCdbCache.wcache <- ""
        (fun() -> BestCdbCache.LoadWhite()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> BestCdbCache.LoadWhite()|>ignore) |> should (throwWithMessage "White Cache file not defined") typeof<System.Exception>

    [<Test>]
    let LoadWhiteDuff() =
        let testwhite = @"D:\Github\GenRep\TestData\DuffCache.txt"
        (fun() -> BestCdb.SetupWhite testwhite) |> should throw typeof<System.Exception>
        (fun() -> BestCdb.SetupWhite testwhite) |> should (throwWithMessage "Invalid line in White Cache:\"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",,,[\"e4\";\"d4\"]") typeof<System.Exception>
        //BestCache.WhiteCache <- ""

    [<Test>]
    let LoadWhite() =
        let testwhite = @"D:\Github\GenRep\TestData\BestCdbCache.txt"
        BestCdb.SetupWhite testwhite
        let ans = BestCdbCache.LoadWhite()
        //BestCache.WhiteCache <- ""
        ans.Count|> should equal 1
        let ans0 = ans["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"]
        ans0.Best|>should equal "d2d4"
        ans0.Score|>should equal 1
        ans0.Rank|>should equal 2

    [<Test>]
    let LoadBlackUndefined() =
        BestCdbCache.bcache <- ""
        (fun() -> BestCdbCache.LoadBlack()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> BestCdbCache.LoadBlack()|>ignore) |> should (throwWithMessage "Black Cache file not defined") typeof<System.Exception>

    [<Test>]
    let LoadBlackDuff() =
        let testblack = @"D:\Github\GenRep\TestData\DuffCache.txt"
        (fun() -> BestCdb.SetupBlack testblack) |> should throw typeof<System.Exception>
        (fun() -> BestCdb.SetupBlack testblack) |> should (throwWithMessage "Invalid line in Black Cache:\"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",,,[\"e4\";\"d4\"]") typeof<System.Exception>
        //BestCache.BlackCache <- ""

    [<Test>]
    let LoadBlack() =
        let testblack = @"D:\Github\GenRep\TestData\BestCdbCache.txt"
        BestCdb.SetupBlack testblack
        let ans = BestCdbCache.LoadBlack()
        //BestCache.BlackCache <- ""
        ans.Count|> should equal 1
        let ans0 = ans["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"]
        ans0.Best|>should equal "d2d4"
        ans0.Score|>should equal 1
        ans0.Rank|>should equal 2
