open GenRepLib
open System.IO

[<EntryPoint>]
let main argv =
    (*
    SETUP
    1. Create a folder e.g. D:\Book
    2. In the folder create base pgn file such as Grobbase.pgn
    3. Specify the book folder
    4. Setup the cache files
    *)
    Book.bkfol <- @"D:\Github\GenRep\Book"
    Book.setcache()
    (*
    Get array of chapters
    EXTEND
    1. Firstly add the set of responses
    2. Add the number of extended best moves and responses required
    3. Add the concluding move for each line
    4. Save the created chapter
    *)
    let num = 10
    let bgms = Book.BaseGames()
    let expand bgm =
        let gm1 = Book.Add1st bgm
        let rec addresps ct igm =
            if ct = num then igm
            else
                let ogm = Book.AddResps igm
                addresps (ct+1) ogm
        let gmr = addresps 0 gm1
        let gm = Book.AddLast gmr
        gm
    let gms = bgms|>List.map expand  
    Book.SaveBook gms
    (*
    To create a kindle book you need to:
    1. Create a markdown file with title of book for White and Black
    2. Specify the templates and images folders
    3. Run code to create OPF files and html
    4. Use either Kindle Previewer or kindlegen to create mobi files
    
    *)
    Kindle.tfol <- @"D:\Github\GenRep\Templates"
    Kindle.ifol <- @"D:\Github\GenRep\Images"
    Book.bkfil|>Path.GetFileNameWithoutExtension|>Kindle.BookOpf






















    0 // return an integer exit code        
