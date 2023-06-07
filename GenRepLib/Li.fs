namespace GenRepLib

open FsChessPgn.OpenExp

module Li =
    let GetMoves (fen:string) =
        let res = Results.Load(addr + fen)
        let mvs = res.Moves
        let sans = mvs|>Array.map(fun m -> m.San)
        sans
    
