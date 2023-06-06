namespace FsChessPgn

open FsChess
open System.IO

module Best =
    type Bmresps = {BestMove:string;Replies:string[]}
    ///load dictionary given array
    let LoadDict(lns:string[]) = 
        let mutable ans = Map.empty
        let doln (ln:string) =
            let bits = ln.Split([|'|'|])
            let fen = bits.[0]
            let bm = bits.[1]
            let rs = if bits.[2]= "" then [||] else bits.[2].Split([|','|])
            let bmrs = {BestMove=bm;Replies=rs}
            ans <- ans.Add(fen,bmrs)
        lns|>Array.iter doln
        ans

    ///get dictionary given location
    let GetDict(fl:string) = 
        if File.Exists(fl) then
            let lns = File.ReadAllLines(fl)
            lns|>LoadDict
        else
            Map.empty

    ///save dictionary given location
    let SaveDict(fl:string, dct:Map<string,Bmresps>) =
        let toln k (v:Bmresps) =
            k + "|" + v.BestMove + "|" + 
            (if v.Replies.Length=0 then "" else v.Replies|>Array.reduce(fun a b -> a + "," + b))
        let lns = dct|>Seq.map(fun (KeyValue(k,v)) -> toln k v)|>Seq.toArray
        File.WriteAllLines(fl,lns)

    ///Add to dictionary given dictionary, fen, best move and list of responses
    let Add(dct:Map<string,Bmresps>, fen:string, bm: string, sans:string[]) =
        let bmrs = {BestMove=bm;Replies=sans}
        if not (dct.ContainsKey(fen)) then dct.Add(fen,bmrs)
        else dct
    
    ///Expand for one move given dictionary,board, depth and move
    let ExpandMove(dct:Map<string,Bmresps>, inbd:Brd, depth:int) (mvstr:string) =
        let mv = mvstr|>MoveUtil.fromSAN inbd
        let bd = inbd|>Board.MoveApply mv
        //need to only process if not already in dct
        let fen = bd|>Board.ToStr
        if dct.ContainsKey fen then dct
        else
            let bm = Leela.GetBestMove(bd,depth)
            let nbd = bd|>Board.MoveApply bm
            let bmstr = bm|>MoveUtil.toPgn bd
            let sans = OpenExp.GetMoves(nbd)
            Add(dct,fen,bmstr,sans)

    ///Expand for one key given dictionary, depth and key value
    let ExpandKey(dct:Map<string,Bmresps>, depth:int) (kv:System.Collections.Generic.KeyValuePair<string,Bmresps>)=
        let fen,bmrs = kv|>fun (KeyValue(k,v)) -> k,v
        
        //printfn "Fen: %s" fen

        let bd = fen|>Board.FromStr
        let nbd = 
            if bmrs.BestMove = "" then bd
            else
                let bm = bmrs.BestMove|>MoveUtil.fromSAN bd
                bd|>Board.MoveApply bm
        let rec expmvs idct (rl:string list) =
            if rl.IsEmpty then idct
            else
                let r = rl.Head

                //printfn "Resp: %s" r

                let odct = ExpandMove(idct,nbd,depth) r
                expmvs odct (rl.Tail)

        expmvs dct (bmrs.Replies|>List.ofArray)

    ///Expand all given dictionary and depth
    let Expand(dct:Map<string,Bmresps>, depth:int) =
        let rec expall idct (kvl:System.Collections.Generic.KeyValuePair<string,Bmresps> list) =
            if kvl.IsEmpty then idct
            else
                let kv = kvl.Head
                let odct = ExpandKey(idct,depth) kv
                expall odct kvl.Tail
        let kvl = dct|>Seq.toList
        expall dct kvl

    ///Add for one fen given dictionary, depth and fen
    let AddFen(dct:Map<string,Bmresps>, depth:int) (fen:string) =
        if dct.ContainsKey fen then dct
        else
            let bd = fen|>Board.FromStr
            let bm = Leela.GetBestMove(bd,depth)
            let nbd = bd|>Board.MoveApply bm
            let bmstr = bm|>MoveUtil.toPgn bd
            let sans = OpenExp.GetMoves(nbd)
            Add(dct,fen,bmstr,sans)
