namespace GenRepTest

open NUnit.Framework
open FsUnit
open GenRepLib

module Rep =
    
    [<SetUp>]
    let Setup () =
        Rep.fol <- @"D:\Github\GenRep\Rep"
        Rep.setcache()
        Best.depth <- 5
        ()

    [<Test>]
    let AddWhite1st() =
        let testw = "Ch1"
        let ngm = Rep.AddWhite1st testw
        //Rep.SaveWhite testw ngm
        ngm.MoveText.Length|>should equal 11
        
    [<Test>]
    let AddBlack1st() =
        let testb = "Ch1"
        let ngm = Rep.AddBlack1st testb
        //Rep.SaveBlack testb ngm
        ngm.MoveText.Length|>should equal 8
        
    [<Test>]
    let AddWhiteResps() =
        let testw = "Ch1"
        let gm1 = Rep.AddWhite1st testw
        let gmr = Rep.AddWhiteResps gm1
        //Rep.SaveWhite testw gmr
        gmr.MoveText.Length|>should equal 17

    [<Test>]
    let AddBlackResps() =
        let testb = "Ch1"
        let gm1 = Rep.AddBlack1st testb
        let gmr = Rep.AddBlackResps gm1
        //Rep.SaveBlack testb gmr
        gmr.MoveText.Length|>should equal 10

    [<Test>]
    let AddWhiteLast() =
        let testw = "Ch1"
        let gm1 = Rep.AddWhite1st testw
        let gmr = Rep.AddWhiteResps gm1
        let gm = Rep.AddWhiteLast gmr
        Rep.SaveWhite testw gm
        gm.MoveText.Length|>should equal 20

    [<Test>]
    let AddBlackLast() =
        let testb = "Ch1"
        let gm1 = Rep.AddBlack1st testb
        let gmr = Rep.AddBlackResps gm1
        let gm = Rep.AddBlackLast gmr
        Rep.SaveBlack testb gm
        gm.MoveText.Length|>should equal 13
