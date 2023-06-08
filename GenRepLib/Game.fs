namespace GenRepLib

open FsChess

module Game =
    
    let GetLastFens (gm:Game) =
        //[2;3;5] indicates go to RAV at index 2, within this go to RAV at index 3 and then get item at index 5
        ()