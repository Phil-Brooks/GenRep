open GenRepLib

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
    (*
    Now have repertoires you can load into Chessable
    EXTEND
    1. Using vacant slot create course using Tools -> Create a Course
    2. Give it a suitable name and specifiy relevant pieces for the repertoire
    3. Create new empty chapters on chessable - Ch1..., Ch2... 
    4. Import each chapter to the relevant target
    4. Save the created chapter
    *)


    (*
    To create a kindle book you need to:
    1. Create a markdown file with title of book for White and Black
    2. Specify the templates and images folders
    3. Run code to create OPF files and html
    4. Use either Kindle Previewer or kindlegen to create mobi files
    
    *)
    Kindle.tfol <- @"D:\Github\GenRep\Templates"
    Kindle.ifol <- @"D:\Github\GenRep\Images"
    Kindle.WhiteOpf "GrobWhite"
    Kindle.BlackOpf "GrobBlack"

    0 // return an integer exit code        
