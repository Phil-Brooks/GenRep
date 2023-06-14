namespace GenRepLib

open System.Threading
open System.Collections.Generic
open FsChessPgn.OpenExp

module Resp =
    let mutable lim = 10
    let mutable wdict = new Dictionary<string, string list>()
    let mutable bdict = new Dictionary<string, string list>()

    let SetupWhite(wfil) = 
        RespCache.wcache <- wfil
        wdict <- RespCache.LoadWhite()

    let SetupBlack(bfil) = 
        RespCache.bcache <- bfil
        bdict <- RespCache.LoadBlack()

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
        let all =
            if wdict.ContainsKey fen then wdict[fen]
            else
                let ans = LiGet fen
                wdict.Add(fen,ans)
                RespCache.SaveWhite(wdict)
                ans
        if all.Length>lim then
            all.[..lim-1]    
        else all
    
    let GetBlack (fen:string) =
        let all =
            if bdict.ContainsKey fen then bdict[fen]
            else
                let ans = LiGet fen
                bdict.Add(fen,ans)
                RespCache.SaveBlack(bdict)
                ans
        if all.Length>lim then
            all.[..lim-1]    
        else all
