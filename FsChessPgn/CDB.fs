namespace FsChessPgn

open FsChess
open System.IO
open FSharp.Data

//getting data from Chess Cloud Database Query Interface
module CDB =
    [<Literal>]
    let sample = "http://www.chessdb.cn/cdb.php?action=queryall&board=rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
    
    let addr = "http://www.chessdb.cn/cdb.php?action=queryall&board="
    

    ///get moves for fen
    let GetMoves(fen:string) = 
        let res = Http.RequestString(addr + fen)
        let mvs = res.Split("|")
        let sans = mvs|>Array.map(fun m -> m.Split(","))
        sans
    