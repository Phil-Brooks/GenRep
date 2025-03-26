namespace GenRepLib

open System.Threading
open System.Collections.Generic
open FsChessPgn.CDB

module BestCdb =
    let mutable wdict = new Dictionary<string, BestCdbEntry>()
    let mutable bdict = new Dictionary<string, BestCdbEntry>()

    let SetupWhite(wfil) = 
        BestCdbCache.wcache <- wfil
        wdict <- BestCdbCache.LoadWhite()

    let SetupBlack(bfil) = 
        BestCdbCache.bcache <- bfil
        bdict <- BestCdbCache.LoadBlack()

    let CdbGet (fen:string) =
        let rec tryget ct =
            try
                let res = GetMoves(fen)
                let ms = res[0]
                let best = ms[0].Split(":")[1]
                let score = int(ms[1].Split(":")[1])
                let rank = int(ms[2].Split(":")[1])
                {Best=best;Score=score;Rank=rank}
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
            let ans = CdbGet fen
            wdict.Add(fen,ans)
            BestCdbCache.SaveWhite(wdict)
            ans

    let GetBlack (fen:string) =
        if bdict.ContainsKey fen then bdict[fen]
        else
            let ans = CdbGet fen
            bdict.Add(fen,ans)
            BestCdbCache.SaveBlack(bdict)
            ans
