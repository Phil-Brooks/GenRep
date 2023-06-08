namespace GenRepLib

open FsChess

module Pgn =
    let Load (pgn:string) =
        IO.ReadFromFile pgn

    let Save (pgn:string) (pgnGame:Game) =
        IO.WriteFile pgn pgnGame
