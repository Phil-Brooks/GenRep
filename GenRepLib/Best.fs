namespace GenRepLib

open FlounderLib

module Best =
    let mutable depth = 5
    let Calc (fenstr:string) =
        //utility to get san
        let Sans (mv:OrdMoveEntryRec) (rs:OrdMoveEntryRec) =
            let uci = OrdMove.ToStr mv
            let ruci = OrdMove.ToStr rs
            let fen = FsChessPgn.FEN.Parse fenstr
            let bd = FsChessPgn.Board.FromFEN fen
            let pmv = FsChessPgn.MoveUtil.fromUci bd uci
            let san = FsChessPgn.MoveUtil.toPgn bd pmv
            let nbd = FsChessPgn.Board.MoveApply pmv bd
            let rpmv = FsChessPgn.MoveUtil.fromUci nbd ruci
            let rsan = FsChessPgn.MoveUtil.toPgn nbd rpmv
            san,rsan
        //setup
        Brd <- Board.FromFen(fenstr)
        NNUEb.ResetRefreshTable()
        NNUEb.AccIndex<-0
        NNUEb.ResetAccumulator(White)
        NNUEb.ResetAccumulator(Black)
        Search.Reset()
        RepHist.Reset()
        //added
        //TranTable.Reset()
        //get best
        let rec getbm cureval curdepth =
            if not (curdepth > depth) then
                let eval = Search.AspirationSearch(curdepth, cureval)
                getbm eval (curdepth + 1)
            else cureval
        let eval = getbm -100000000 1                    
        let bm = PrincVars.Get(0)
        let resp = PrincVars.Get(1)
        let sbm,sresp = Sans bm resp
        {Best=sbm;Resp=sresp;Eval=eval}
    
    let GetWhite (fen:string) =
        let dict = BestCache.LoadWhite()
        if dict.ContainsKey fen then dict[fen]
        else
            let ans = Calc fen
            dict.Add(fen,ans)
            BestCache.SaveWhite(dict)
            ans

    let GetBlack (fen:string) =
        let dict = BestCache.LoadBlack()
        if dict.ContainsKey fen then dict[fen]
        else
            let ans = Calc fen
            dict.Add(fen,ans)
            BestCache.SaveBlack(dict)
            ans
