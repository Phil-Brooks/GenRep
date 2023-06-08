namespace GenRepLib

open System.IO
open System.Collections.Generic

module BestCache =
    let mutable WhiteCache = ""
    let mutable BlackCache = ""
    
    let SaveWhite (wd:BestCacheDict) =
        let b2str (b:BestEntry) =
            "{Best:" + b.Best + ";Resp:" + b.Resp + ";Eval:" + b.Eval.ToString() + "}"
        let lines =
            wd
            |> Seq.map(fun (KeyValue(k,v)) -> "\"" + k + "\",[" + (b2str v) + "]")
        if WhiteCache = "" then failwith "White Cache file not defined"
        else File.WriteAllLines(WhiteCache,lines)
    
    let SaveBlack (bd:BestCacheDict) =
        let b2str (b:BestEntry) =
            "{Best:" + b.Best + ";Resp:" + b.Resp + ";Eval:" + b.Eval.ToString() + "}"
        let lines =
            bd
            |> Seq.map(fun (KeyValue(k,v)) -> "\"" + k + "\"," + (b2str v))
        if BlackCache = "" then failwith "Black Cache file not defined"
        else File.WriteAllLines(BlackCache,lines)

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
                let resp = ms[1].Split(":")[1]
                let eval = int(ms[2].Split(":")[1])
                let v = {Best=best;Resp=resp;Eval=eval}
                k,v

        if WhiteCache = "" then failwith "White Cache file not defined"
        else 
            if File.Exists(WhiteCache) then
                let lines = File.ReadAllLines(WhiteCache)
                lines|>Array.map ln2tuple|>dict|>Dictionary
            else new Dictionary<string, BestEntry>()

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
                let resp = ms[1].Split(":")[1]
                let eval = int(ms[2].Split(":")[1])
                let v = {Best=best;Resp=resp;Eval=eval}
                k,v

        if BlackCache = "" then failwith "Black Cache file not defined"
        else 
            if File.Exists(BlackCache) then
                let lines = File.ReadAllLines(BlackCache)
                lines|>Array.map ln2tuple|>dict|>Dictionary
            else new Dictionary<string, BestEntry>()
