﻿namespace GenRepLib

open FlounderLib

module Best =
    let Get (depth:int) (fenstr:string) =
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
        NNUEb.ResetRefreshTable()
        NNUEb.AccIndex<-0
        NNUEb.ResetAccumulator(White)
        NNUEb.ResetAccumulator(Black)
        Search.Reset()
        Brd <- Board.FromFen(fenstr)
        RepHist.Reset()
        //get best
        let mutable bm = OrdMove.Default
        let mutable resp = OrdMove.Default
        let rec getbm cureval curdepth =
            if not (curdepth > depth) then
                let eval = Search.AspirationSearch(curdepth, cureval)
                bm <- PrincVars.Get(0)
                resp <- PrincVars.Get(1)
                getbm eval (curdepth + 1)
            else cureval
        let eval = getbm -100000000 1                    
        let sbm,sresp = Sans bm resp
        {Best=sbm;Resp=sresp;Eval=eval}
    
