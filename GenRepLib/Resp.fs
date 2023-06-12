namespace GenRepLib

open System.Threading
open FsChessPgn.OpenExp

module Resp =
    let LiGet (fen:string) =
        let rec tryget ct =
            try
                let res = Results.Load(addr + fen)
                let mvs = res.Moves
                let sans = mvs|>Array.map(fun m -> m.San)
                sans|>List.ofArray
            with
                | ex ->
                    printfn "Fail probably 429, count: %i"  ct
                    if ct<4 then
                        Thread.Sleep(ct*100)
                        tryget (ct+1)
                    else failwith"too many tries"
        tryget 1
    
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
