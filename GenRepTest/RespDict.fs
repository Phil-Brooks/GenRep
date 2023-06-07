namespace GenRepTest

open System.Collections.Generic
open NUnit.Framework
open FsUnit

module RespDict =
    
    [<SetUp>]
    let Setup () =
        ()

    [<Test>]
    let Add() =
        let testdict = dict["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",["e4";"d4"]]|>Dictionary
        let fen = "dummy"
        let mvl = ["dummy1";"dummy2"]
        testdict.Add(fen,mvl)
        testdict.Count|>should equal 2

    [<Test>]
    let RemoveDummy() =
        let testdict = dict["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",["e4";"d4"]]|>Dictionary
        let fen = "dummy"
        let ans = testdict.Remove(fen)
        ans|>should equal false
        testdict.Count|>should equal 1

    [<Test>]
    let Remove() =
        let testdict = dict["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",["e4";"d4"]]|>Dictionary
        let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        let ans = testdict.Remove(fen)
        ans|>should equal true
        testdict.Count|>should equal 0
    