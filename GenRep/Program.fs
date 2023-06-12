﻿open GenRepLib

[<EntryPoint>]
let main argv =
    (*
    SETUP
    1. Create a folder e.g. D:\Rep
    2. Create subfolders White and Black
    3. In each folder create base chapter pgn files such as CH1base.pgn,Ch2base.pgn
    4. Specify the rep folder
    5. Setup the cache files
    6. Set the depth for getting the best moves
    *)
    Rep.fol <- @"D:\Github\GenRep\Rep"
    Rep.setcache()
    Best.depth <- 5
    (*
    Get array of chapters
    EXTEND
    1. Firstly add the set of responses
    2. Add the number of extended best moves and responses required
    3. Add the concluding move for each line
    4. Save the created chapter
    *)
    let num = 10
    let wchs = Rep.WhiteChapters()
    for ch in wchs do
        let gm1 = Rep.AddWhite1st ch
        let rec addresps ct igm =
            if ct = num then igm
            else
                let ogm = Rep.AddWhiteResps igm
                addresps (ct+1) ogm
        let gmr = addresps 0 gm1
        let gm = Rep.AddWhiteLast gmr
        Rep.SaveWhite ch gm
    let bchs = Rep.BlackChapters()
    for ch in bchs do
        let gm1 = Rep.AddBlack1st ch
        let rec addresps ct igm =
            if ct = num then igm
            else
                let ogm = Rep.AddBlackResps igm
                addresps (ct+1) ogm
        let gmr = addresps 0 gm1
        let gm = Rep.AddBlackLast gmr
        Rep.SaveBlack ch gm












    0 // return an integer exit code        
