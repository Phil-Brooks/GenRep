namespace GenRepTest

open NUnit.Framework
open FsUnit
open GenRep

module Li =

    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let GetInitial() =
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let ans = Li.GetMoves fen
        ans.[0] |> should equal "e4"
        ans.Length|> should equal 12

    [<Test>]
    let GetFrench() =
        let fen = "rnbqkbnr/ppp2ppp/4p3/3p4/3PP3/8/PPP2PPP/RNBQKBNR w KQkq - 0 3"
        let ans = Li.GetMoves fen
        ans.[0] |> should equal "Nc3"
        ans.Length|> should equal 12

    [<Test>]
    let GetNone() =
        let fen = "rnbqkbnr/ppp1pppp/8/3p4/1P4P1/8/P1PPPP1P/RNBQKBNR b KQkq - 0 2"
        let ans = Li.GetMoves fen
        ans.Length|> should equal 0
