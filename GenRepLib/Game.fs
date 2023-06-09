namespace GenRepLib

open FsChess

module Game =
    
    let AddBest (depth:int) (gm:Game) =
        //Just go to end of each var and add another move using best cache and score as NAG
        let rec addbest (imtel:MoveTextEntry list) (omtel:MoveTextEntry list) =
            if imtel.IsEmpty then 
                //need to remove iscont and mn if black, if after a move
                match omtel.Head with
                |HalfMoveEntry(_,_,_,amv) -> 
                    let brd = amv.Value.PostBrd
                    let fen = FsChessPgn.FEN.FromBd brd|>FsChessPgn.FEN.ToStr
                    let be = if brd.WhosTurn=Player.White then Best.GetWhite depth fen else Best.GetBlack depth fen
                    let bm = be.Best
                    let eval = be.Eval
                    []
                |_ -> failwith "last move should be a move"
            else
                let mte = imtel.Head
                match mte with
                |GameEndEntry(_) -> 
                    match omtel.Head with
                    |HalfMoveEntry(_,_,_,amv) -> 
                        let brd = amv.Value.PostBrd
                        let fen = FsChessPgn.FEN.FromBd brd|>FsChessPgn.FEN.ToStr
                        let be = if brd.WhosTurn=Player.White then Best.GetWhite depth fen else Best.GetBlack depth fen
                        let bm = be.Best
                        let eval = be.Eval
                        let pmv = FsChessPgn.pMove.Parse bm
                        let nmte = HalfMoveEntry(None,false,pmv,None)
                        (mte::nmte::omtel)|>List.rev

                    |_ -> failwith "last move should be a move"
                |RAVEntry(_) -> 
                    //need to process this and replace with extended version
                    []
                |_ -> addbest imtel.Tail (imtel.Head::omtel)
        let agm = Game.GetaMoves gm
        let nmt = addbest agm.MoveText []
        {gm with MoveText=nmt}

    let AddResps (gm:Game) =
        //Just go to end of each var and add best move using best cache 
        //and then list of moves using resp cache with diags
        ()