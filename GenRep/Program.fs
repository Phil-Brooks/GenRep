open GenRepLib

[<EntryPoint>]
let main argv =
    let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
    Best.depth <- 5
    let ans = Best.Calc fen
    let ans = Best.Calc fen
    0 // return an integer exit code        
