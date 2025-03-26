namespace GenRepLib

open System.IO
open System.Collections.Generic

module BestCdbCache =
    let mutable wcache = ""
    let mutable bcache = ""
    
    let SaveWhite (wd:BestCdbCacheDict) =
        let b2str (b:BestCdbEntry) =
            "{Best:" + b.Best + ";Resp:" + b.Score.ToString() + ";Eval:" + b.Rank.ToString() + "}"
        let lines =
            wd
            |> Seq.map(fun (KeyValue(k,v)) -> "\"" + k + "\"," + (b2str v))
        if wcache = "" then failwith "White Cache file not defined"
        else File.WriteAllLines(wcache,lines)
    
    let SaveBlack (bd:BestCdbCacheDict) =
        let b2str (b:BestCdbEntry) =
            "{Best:" + b.Best + ";Resp:" + b.Score.ToString() + ";Eval:" + b.Rank.ToString() + "}"
        let lines =
            bd
            |> Seq.map(fun (KeyValue(k,v)) -> "\"" + k + "\"," + (b2str v))
        if bcache = "" then failwith "Black Cache file not defined"
        else File.WriteAllLines(bcache,lines)

    let LoadWhite () =
        let ln2tuple (l:string) =
            let bits = l.Split(",")
            if bits.Length<>2 then failwith ("Invalid line in White Cache:" + l)
            else
                let k = bits[0].Trim('\"')
                let v0:string = bits[1].Trim('{').Trim('}')
                let ms = v0.Split(";")
                if ms.Length<>3 then failwith ("Invalid line in White Cache:" + l)
                let best = ms[0].Split(":")[1]
                let score = int(ms[1].Split(":")[1])
                let rank = int(ms[2].Split(":")[1])
                let v = {Best=best;Score=score;Rank=rank}
                k,v

        if wcache = "" then failwith "White Cache file not defined"
        else 
            if File.Exists(wcache) then
                let lines = File.ReadAllLines(wcache)
                lines|>Array.map ln2tuple|>dict|>Dictionary
            else new Dictionary<string, BestCdbEntry>()

    let LoadBlack () =
        let ln2tuple (l:string) =
            let bits = l.Split(",")
            if bits.Length<>2 then failwith ("Invalid line in Black Cache:" + l)
            else
                let k = bits[0].Trim('\"')
                let v0:string = bits[1].Trim('{').Trim('}')
                let ms = v0.Split(";")
                if ms.Length<>3 then failwith ("Invalid line in Black Cache:" + l)
                let best = ms[0].Split(":")[1]
                let score = int(ms[1].Split(":")[1])
                let rank = int(ms[2].Split(":")[1])
                let v = {Best=best;Score=score;Rank=rank}
                k,v

        if bcache = "" then failwith "Black Cache file not defined"
        else 
            if File.Exists(bcache) then
                let lines = File.ReadAllLines(bcache)
                lines|>Array.map ln2tuple|>dict|>Dictionary
            else new Dictionary<string, BestCdbEntry>()
