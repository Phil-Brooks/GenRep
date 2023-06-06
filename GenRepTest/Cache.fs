namespace GenRepTest

open System.IO
open NUnit.Framework
open FsUnit
open GenRep

module Cache =

    let testdict = dict["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",["e4";"d4"]]
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let SaveWhiteUndefined() =
        (fun() -> Cache.SaveWhite testdict) |> should throw typeof<System.Exception>
        (fun() -> Cache.SaveWhite testdict) |> should (throwWithMessage "White Cache file not defined") typeof<System.Exception>

    [<Test>]
    let SaveWhite() =
        let testwhite = @"D:\Github\GenRep\TestData\TempCache.txt"
        Cache.WhiteCache <- testwhite
        Cache.SaveWhite testdict
        File.Exists testwhite|>should equal true
        Cache.WhiteCache <- ""
        File.Delete testwhite

    [<Test>]
    let SaveBlackUndefined() =
        (fun() -> Cache.SaveBlack testdict) |> should throw typeof<System.Exception>
        (fun() -> Cache.SaveBlack testdict) |> should (throwWithMessage "Black Cache file not defined") typeof<System.Exception>

    [<Test>]
    let SaveBlack() =
        let testblack = @"D:\Github\GenRep\TestData\TempCache.txt"
        Cache.BlackCache <- testblack
        Cache.SaveBlack testdict
        File.Exists testblack|>should equal true
        Cache.BlackCache <- ""
        File.Delete testblack

    [<Test>]
    let LoadWhiteUndefined() =
        (fun() -> Cache.LoadWhite()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> Cache.LoadWhite()|>ignore) |> should (throwWithMessage "White Cache file not defined") typeof<System.Exception>

    [<Test>]
    let LoadWhiteDuff() =
        let testwhite = @"D:\Github\GenRep\TestData\DuffCache.txt"
        Cache.WhiteCache <- testwhite
        (fun() -> Cache.LoadWhite()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> Cache.LoadWhite()|>ignore) |> should (throwWithMessage "Invalid line in White Cache:\"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",,,[\"e4\";\"d4\"]") typeof<System.Exception>
        Cache.WhiteCache <- ""

    [<Test>]
    let LoadWhite() =
        let testwhite = @"D:\Github\GenRep\TestData\Cache.txt"
        Cache.WhiteCache <- testwhite
        let ans = Cache.LoadWhite()
        Cache.WhiteCache <- ""
        ans.Count|> should equal 1
        let ans0 = ans["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"]
        ans0.Length|>should equal 2
        ans0[1]|>should equal "d4"

    [<Test>]
    let LoadBlackUndefined() =
        (fun() -> Cache.LoadBlack()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> Cache.LoadBlack()|>ignore) |> should (throwWithMessage "Black Cache file not defined") typeof<System.Exception>

    [<Test>]
    let LoadBlackDuff() =
        let testblack = @"D:\Github\GenRep\TestData\DuffCache.txt"
        Cache.BlackCache <- testblack
        (fun() -> Cache.LoadBlack()|>ignore) |> should throw typeof<System.Exception>
        (fun() -> Cache.LoadBlack()|>ignore) |> should (throwWithMessage "Invalid line in Black Cache:\"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\",,,[\"e4\";\"d4\"]") typeof<System.Exception>
        Cache.BlackCache <- ""

    [<Test>]
    let LoadBlack() =
        let testblack = @"D:\Github\GenRep\TestData\Cache.txt"
        Cache.BlackCache <- testblack
        let ans = Cache.LoadBlack()
        Cache.BlackCache <- ""
        ans.Count|> should equal 1
        let ans0 = ans["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"]
        ans0.Length|>should equal 2
        ans0[1]|>should equal "d4"