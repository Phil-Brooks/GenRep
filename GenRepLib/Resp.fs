namespace GenRepLib

open System.Threading
open System.Collections.Generic
open FsChessPgn.OpenExp

module Resp =
    let mutable wdict = new Dictionary<string, string list>()
    let mutable bdict = new Dictionary<string, string list>()
    let mutable bkdict = new Dictionary<string, string list>()

    let SetupWhite(wfil) = 
        RespCache.wcache <- wfil
        wdict <- RespCache.LoadWhite()

    let SetupBlack(bfil) = 
        RespCache.bcache <- bfil
        bdict <- RespCache.LoadBlack()

    let SetupBook(bkfil) = 
        RespCache.bkcache <- bkfil
        bkdict <- RespCache.LoadBook()

    let LiGet (fen:string) =
        let rec tryget ct =
            try
                let res = Results.Load(addr + fen)
                let tot = res.White + res.Draws + res.Black
                let mvs = res.Moves
                //only pick > 25%
                let sans = 
                    mvs
                    |>Array.map(fun m -> float(m.White + m.Draws + m.Black)/float(tot),m.San)
                    |>Array.filter(fun (p,s) -> p > 0.25)
                    |>Array.map snd
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
        if wdict.ContainsKey fen then wdict[fen]
        else
            let ans = LiGet fen
            wdict.Add(fen,ans)
            RespCache.SaveWhite(wdict)
            ans
    
    let GetBlack (fen:string) =
        if bdict.ContainsKey fen then bdict[fen]
        else
            let ans = LiGet fen
            bdict.Add(fen,ans)
            RespCache.SaveBlack(bdict)
            ans

    let GetBook (fen:string) =
        if bkdict.ContainsKey fen then bkdict[fen]
        else
            let ans = LiGet fen
            bkdict.Add(fen,ans)
            RespCache.SaveBook(bkdict)
            ans
