namespace GenRepLib

open FsChess

module Pgn =
    let Load (pgn:string) =
        let gm = IO.ReadFromFile pgn
        Game.GetaMoves gm

    let Save (pgn:string) (pgnGame:Game) =
        IO.WriteFile pgn pgnGame
