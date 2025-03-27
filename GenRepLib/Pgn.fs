namespace GenRepLib

open FsChess

module Pgn =
    let Load (pgn:string) =
        let gm = IO.ReadFromFile pgn
        Game.GetaMoves gm

    let LoadList (pgn:string) =
        let gml = IO.ReadListFromFile pgn
        gml|>List.map Game.GetaMoves

    let Save (pgn:string) (pgnGame:Game) =
        IO.WriteGame pgn pgnGame

    let SaveList (pgn:string) (pgnGames:Game list) =
        IO.WriteFile pgn pgnGames
