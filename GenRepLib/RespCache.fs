namespace GenRepLib

open System.IO
open System.Collections.Generic

module RespCache =
    let mutable wcache = ""
    let mutable bcache = ""
    
    let SaveWhite (wd:RespCacheDict) =
        let l2str (l:string list) =
            if l.IsEmpty then ""
            else
                l
                |>List.map (fun m -> "\"" + m + "\"")
                |>List.reduce (fun a b -> a + ";" + b)
        let lines =
            wd
            |> Seq.map(fun (KeyValue(k,v)) -> "\"" + k + "\",[" + (l2str v) + "]")
        if wcache = "" then failwith "White Cache file not defined"
        else File.WriteAllLines(wcache,lines)
    
    let SaveBlack (bd:RespCacheDict) =
        let l2str (l:string list) =
            if l.IsEmpty then ""
            else
                l
                |>List.map (fun m -> "\"" + m + "\"")
                |>List.reduce (fun a b -> a + ";" + b)
        let lines =
            bd
            |> Seq.map(fun (KeyValue(k,v)) -> "\"" + k + "\",[" + (l2str v) + "]")
        if bcache = "" then failwith "Black Cache file not defined"
        else File.WriteAllLines(bcache,lines)

    let LoadWhite () =
        let ln2tuple (l:string) =
            let bits = l.Split(",")
            if bits.Length<>2 then failwith ("Invalid line in White Cache:" + l)
            else
                let k = bits[0].Trim('\"')
                let v0:string = bits[1].Trim('[').Trim(']')
                if v0="" then 
                    k,[]
                else
                    let ms = v0.Split(";")
                    let v = ms|>Array.map(fun m -> m.Trim('\"'))|>Array.toList
                    k,v

        if wcache = "" then failwith "White Cache file not defined"
        else 
            if File.Exists(wcache) then
                let lines = File.ReadAllLines(wcache)
                lines|>Array.map ln2tuple|>dict|>Dictionary
            else new Dictionary<string, string list>()

    let LoadBlack () =
        let ln2tuple (l:string) =
            let bits = l.Split(",")
            if bits.Length<>2 then failwith ("Invalid line in Black Cache:" + l)
            else
                let k = bits[0].Trim('\"')
                let v0:string = bits[1].Trim('[').Trim(']')
                if v0="" then 
                    k,[]
                else
                    let ms = v0.Split(";")
                    let v = ms|>Array.map(fun m -> m.Trim('\"'))|>Array.toList
                    k,v

        if bcache = "" then failwith "Black Cache file not defined"
        else 
            if File.Exists(bcache) then
                let lines = File.ReadAllLines(bcache)
                lines|>Array.map ln2tuple|>dict|>Dictionary
            else new Dictionary<string, string list>()
