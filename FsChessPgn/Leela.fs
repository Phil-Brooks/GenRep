namespace FsChessPgn

open FsChess
open System.IO

module Leela =
    let mutable prc = new System.Diagnostics.Process()
    let mutable LeelaFol = @"c:\lc0"

    let SetFol fol = LeelaFol<-fol
    
    ///send message to engine
    let Send(command:string) = 
        prc.StandardInput.WriteLine(command)
    
    ///set up engine
    let ComputeAnswer(fen, depth) = 
        Send("ucinewgame")
        Send("setoption name Threads value " + (System.Environment.ProcessorCount - 1).ToString())
        Send("position startpos")
        Send("position fen " + fen + " ")
        Send("go depth " + depth.ToString())
        prc.WaitForExit(60000)
    
    ///set up process
    let SetUpPrc () = 
        prc.StartInfo.CreateNoWindow <- true
        prc.StartInfo.FileName <- LeelaFol + "/lc0.exe"
        prc.StartInfo.WorkingDirectory <- LeelaFol
        prc.StartInfo.RedirectStandardOutput <- true
        prc.StartInfo.UseShellExecute <- false
        prc.StartInfo.RedirectStandardInput <- true
        prc.StartInfo.WindowStyle <- System.Diagnostics.ProcessWindowStyle.Hidden
        prc.Start() |> ignore
        prc.BeginOutputReadLine()
    
    ///Gets the Score and Line from a message
    let GetScrLn(msg:string,bd:Brd) =
        if msg.StartsWith("info") then
            let mv = 
                let st = msg.LastIndexOf("pv")
                let ucis = msg.Substring(st+2)
                //need to change to SAN format
                let bits = ucis.Trim().Split([|' '|])//|>Convert.UcisToSans bd mno
                bits.[0]|>MoveUtil.fromUci bd|>MoveUtil.toPgn bd
            let scr =
                let st = msg.LastIndexOf("cp")
                let ss = msg.Substring(st+2,10).Trim()
                let bits = ss.Split([|' '|])
                let cp = float(bits.[0])/100.0
                cp

            scr,mv
        else
            0.0,msg

    //anl
    let Stop() = 
        if prc <> null then prc.Kill()

    let GetBestMove(cbd:Brd,dpth:int) = 
        prc <- new System.Diagnostics.Process()
        
        let mutable bmo = None
        //p_out
        let pOut (e : System.Diagnostics.DataReceivedEventArgs) = 
            if not (e.Data = null || e.Data = "") then 
                let msg = e.Data.ToString().Trim()

                //printfn "Message: %s" msg 
                
                if msg.StartsWith("bestmove") then 
                    
                    //printfn "Message: %s" msg

                    let bits = msg.Split([|' '|])
                    bmo <- (bits.[1]|>MoveUtil.fromUci cbd)|>Some
                    Stop()
                elif msg.StartsWith("info") then
                    let st = msg.LastIndexOf("pv")
                    let ucis = msg.Substring(st+2)
                    let bits = ucis.Trim().Split([|' '|])
                    bmo <- bits.[0]|>MoveUtil.fromUci cbd|>Some
                    //printfn "Best: %s" bits.[0]

        prc.OutputDataReceived.Add(pOut)
        //Start process
        SetUpPrc()
        // call calcs
        // need to send game position moves as UCI
        let fen = cbd|>Board.ToStr
        ComputeAnswer(fen, dpth)|>ignore
        Stop()

        if bmo.IsSome then 
            //printfn "Final Best: %s" (bmo.Value|>MoveUtil.toPgn cbd)
            
            bmo.Value
        else failwith ("NO BEST MOVE FOUND")


