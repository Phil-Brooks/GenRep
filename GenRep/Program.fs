open GenRepLib

[<EntryPoint>]
let main argv =
    let fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
    let ans = Best.Calc 5 fen
    0 // return an integer exit code        
