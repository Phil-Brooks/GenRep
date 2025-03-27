namespace GenRepLib

open System.IO
open FsChess

module Book =
    let mutable bkfol = ""
    let mutable bkfil = ""

    let setcache() = 
        Best.SetupBook(Path.Combine(bkfol,"BestCache"))
        Resp.SetupBook(Path.Combine(bkfol,"RespCache"))

    let BaseGames() =
        let files = Directory.GetFiles(bkfol)
        let pgns = files|>Array.filter(fun f -> f.EndsWith("base.pgn"))
        if pgns.Length<>1 then failwith "should be only one base pgn file"
        else
            let basepgn = pgns[0]
            let outpgn = basepgn|>Path.GetFileNameWithoutExtension|>(fun f -> f.Substring(0,f.Length-4))
            bkfil <- outpgn + ".pgn"
            Pgn.LoadList(basepgn) 
            
    let SaveBook (gms:Game list) =
        let bkfile = Path.Combine(bkfol,bkfil)
        Pgn.SaveList bkfile gms

    let Add1st (gm:Game) =
        //Just go to end of each var and add a set of Ravs using resp cache
        let rec add1st hmn (camv:aMove option) (imtel:MoveTextEntry list) (omtel:MoveTextEntry list) =
            if imtel.IsEmpty then 
                failwith "last move should be a GameEndEntry"
            else
                let mte = imtel.Head
                match mte with
                |GameEndEntry(_) -> 
                    let getrav brd (resp:string) =
                        let pmv = FsChessPgn.pMove.Parse resp
                        let amv = FsChessPgn.pMove.ToaMove brd (hmn/2) pmv
                        let mte = HalfMoveEntry(Some(hmn/2),true,pmv,Some(amv))
                        let ce =CommentEntry("[#]")
                        RAVEntry([mte;ce])
                    let brd = camv.Value.PostBrd
                    let fen = FsChessPgn.FEN.FromBd brd|>FsChessPgn.FEN.ToStr
                    let resps = Resp.GetBook fen
                    if resps.Length=0 then failwith"got no resps"
                    else
                        let pmv = FsChessPgn.pMove.Parse resps.Head
                        let amv = FsChessPgn.pMove.ToaMove brd (hmn/2) pmv
                        let nmte = HalfMoveEntry(None,false,pmv,Some(amv))
                        let ce =CommentEntry("[#]")
                        if resps.Length=1 then
                            (mte::nmte::omtel)|>List.rev
                        else
                            let ravs = resps.Tail|>List.map(getrav brd)|>List.rev
                            (mte::(ravs@[ce;nmte]@omtel))|>List.rev
                |HalfMoveEntry(_,_,_,amv) -> add1st (hmn+1) amv imtel.Tail (imtel.Head::omtel)
                |_ -> add1st hmn camv imtel.Tail (imtel.Head::omtel)
        let nmt = add1st 2 None gm.MoveText []
        {gm with MoveText=nmt}

    let AddResps (gm:Game) =
        //Just go to end of each var and add best move using best cache 
        //and then list of moves using resp cache with diags
        //CHANGE - if not in lichess then no point including as nobody plays it! 
        let rec addresps hmn (camv:aMove option) (imtel:MoveTextEntry list) (omtel:MoveTextEntry list) =
           if imtel.IsEmpty then 
                //this is end of a RAV so can add here
                let brd = camv.Value.PostBrd
                let fen = FsChessPgn.FEN.FromBd brd|>FsChessPgn.FEN.ToStr
                let bm = Best.GetBook fen
                let pmv = FsChessPgn.pMove.Parse bm.Best
                let amv = FsChessPgn.pMove.ToaMove brd (hmn/2) pmv
                let nmte = HalfMoveEntry(Some(hmn/2),false,pmv,Some(amv))
                let nomtel = nmte::omtel
                let getrav brd (resp:string) =
                    let pmv = FsChessPgn.pMove.Parse resp
                    let amv = FsChessPgn.pMove.ToaMove brd (hmn/2) pmv
                    let mte = HalfMoveEntry(Some(hmn/2),true,pmv,Some(amv))
                    let ce = CommentEntry("[#]")
                    RAVEntry([mte;ce])
                let nbrd = amv.PostBrd
                let fen = FsChessPgn.FEN.FromBd nbrd|>FsChessPgn.FEN.ToStr
                let iresps = Resp.GetBook fen
                let tresps = iresps|>List.filter(fun r -> r<>bm.Resp)
                let resps = bm.Resp::tresps
                let pmv = FsChessPgn.pMove.Parse resps.Head
                let amv = FsChessPgn.pMove.ToaMove nbrd (hmn/2) pmv
                let nmte = HalfMoveEntry(None,false,pmv,Some(amv))
                let ce = CommentEntry("[#]")
                if resps.Length=1 then
                    //(nmte::nomtel)|>List.rev
                    //CHANGE
                    omtel|>List.rev
                else
                    let ravs = resps.Tail|>List.map(getrav nbrd)|>List.rev
                    (ravs@[ce;nmte]@nomtel)|>List.rev
            else
                let mte = imtel.Head
                match mte with
                |GameEndEntry(_) ->
                    let brd = camv.Value.PostBrd
                    let fen = FsChessPgn.FEN.FromBd brd|>FsChessPgn.FEN.ToStr
                    let bm = Best.GetBook fen
                    let pmv = FsChessPgn.pMove.Parse bm.Best
                    let amv = FsChessPgn.pMove.ToaMove brd (hmn/2) pmv
                    let nmte = HalfMoveEntry(Some(hmn/2),false,pmv,Some(amv))
                    let nomtel = nmte::omtel
                    let getrav brd (resp:string) =
                        let pmv = FsChessPgn.pMove.Parse resp
                        let amv = FsChessPgn.pMove.ToaMove brd (hmn/2) pmv
                        let mte = HalfMoveEntry(Some(hmn/2),true,pmv,Some(amv))
                        let ce = CommentEntry("[#]")
                        RAVEntry([mte;ce])
                    let nbrd = amv.PostBrd
                    let fen = FsChessPgn.FEN.FromBd nbrd|>FsChessPgn.FEN.ToStr
                    let iresps = Resp.GetBook fen
                    let tresps = iresps|>List.filter(fun r -> r<>bm.Resp)
                    let resps = bm.Resp::tresps
                    let pmv = FsChessPgn.pMove.Parse resps.Head
                    let amv = FsChessPgn.pMove.ToaMove nbrd (hmn/2) pmv
                    let nmte = HalfMoveEntry(None,false,pmv,Some(amv))
                    let ce =CommentEntry("[#]")
                    if resps.Length=1 then
                        //(mte::nmte::nomtel)|>List.rev
                        //CHANGE
                        mte::omtel|>List.rev
                    else
                        let ravs = resps.Tail|>List.map(getrav nbrd)|>List.rev
                        (mte::(ravs@[ce;nmte]@nomtel))|>List.rev
                |RAVEntry(rml) -> 
                    let nrml = addresps (hmn-1) camv rml []
                    let nrav = RAVEntry(nrml)
                    addresps hmn camv imtel.Tail (nrav::omtel)
                |HalfMoveEntry(_,_,_,amv) -> addresps (hmn+1) amv imtel.Tail (imtel.Head::omtel)
                |_ -> addresps hmn camv imtel.Tail (imtel.Head::omtel)
        let nmt = addresps 2 None gm.MoveText []
        {gm with MoveText=nmt}

    let GetNag (eval:int) =
        if eval > -50 && eval < 50 then NAGEntry(NAG.Even)
        elif eval > 0 then
            if eval < 150 then NAGEntry(NAG.Wslight) 
            elif eval < 250 then NAGEntry(NAG.Wmoderate) 
            else NAGEntry(NAG.Wdecisive)
        else
            if eval > -150 then NAGEntry(NAG.Bslight) 
            elif eval > -250 then NAGEntry(NAG.Bmoderate) 
            else NAGEntry(NAG.Bdecisive)

    let AddLast (gm:Game) =
        //Just go to end of each var and add best move using best cache 
        //and then nag based on eval and a diagram
        let rec addlast hmn (camv:aMove option) (imtel:MoveTextEntry list) (omtel:MoveTextEntry list) =
           if imtel.IsEmpty then 
                //this is end of a RAV so can add here
                let brd = camv.Value.PostBrd
                let fen = FsChessPgn.FEN.FromBd brd|>FsChessPgn.FEN.ToStr
                let bm = Best.GetBook fen
                let pmv = FsChessPgn.pMove.Parse bm.Best
                let amv = FsChessPgn.pMove.ToaMove brd (hmn/2) pmv
                let nmte = HalfMoveEntry(Some(hmn/2),false,pmv,Some(amv))
                let ce = CommentEntry("[#]")
                let ng = GetNag(bm.Eval)
                let nomtel = ce::ng::nmte::omtel
                nomtel|>List.rev
            else
                let mte = imtel.Head
                match mte with
                |GameEndEntry(_) ->
                    let brd = camv.Value.PostBrd
                    let fen = FsChessPgn.FEN.FromBd brd|>FsChessPgn.FEN.ToStr
                    let bm = Best.GetBook fen
                    let pmv = FsChessPgn.pMove.Parse bm.Best
                    let amv = FsChessPgn.pMove.ToaMove brd (hmn/2) pmv
                    let nmte = HalfMoveEntry(Some(hmn/2),false,pmv,Some(amv))
                    let ce = CommentEntry("[#]")
                    let ng = GetNag(bm.Eval)
                    let nomtel = mte::ce::ng::nmte::omtel
                    nomtel|>List.rev
                |RAVEntry(rml) -> 
                    let nrml = addlast (hmn-1) camv rml []
                    let nrav = RAVEntry(nrml)
                    addlast hmn camv imtel.Tail (nrav::omtel)
                |HalfMoveEntry(_,_,_,amv) -> addlast (hmn+1) amv imtel.Tail (imtel.Head::omtel)
                |_ -> addlast hmn camv imtel.Tail (imtel.Head::omtel)
        let nmt = addlast 2 None gm.MoveText []
        {gm with MoveText=nmt}

