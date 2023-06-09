namespace GenRepTest

open NUnit.Framework
open FsUnit
open GenRepLib

module Game =
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let AddBestSimple() =
        let testwhite = @"D:\Github\GenRep\TestData\BestCache1.txt"
        BestCache.WhiteCache <- testwhite
        let simple = @"D:\Github\GenRep\TestData\Simple.pgn"
        let gm = Pgn.Load simple
        let ngm = Game.AddBest 5 gm
        ngm.MoveText.Length|>should equal 6//7
        let simple1 = @"D:\Github\GenRep\TestData\Simple1.pgn"
        Pgn.Save simple1 ngm
