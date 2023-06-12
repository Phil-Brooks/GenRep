namespace GenRepLib

open FsChessPgn.OpenExp

module Resp =
    let LiGet (fen:string) =
        let res = Results.Load(addr + fen)
        let mvs = res.Moves
        let sans = mvs|>Array.map(fun m -> m.San)
        sans|>List.ofArray
    
    let GetWhite (fen:string) =
        let dict = RespCache.LoadWhite()
        if dict.ContainsKey fen then dict[fen]
        else
            let ans = LiGet fen
            dict.Add(fen,ans)
            RespCache.SaveWhite(dict)
            ans
    
    let GetBlack (fen:string) =
        let dict = RespCache.LoadBlack()
        if dict.ContainsKey fen then dict[fen]
        else
            let ans = LiGet fen
            dict.Add(fen,ans)
            RespCache.SaveBlack(dict)
            ans
