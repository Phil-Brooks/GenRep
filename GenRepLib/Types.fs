namespace GenRepLib

open System.Collections.Generic

[<AutoOpen>]
module Types =
    type RespCacheDict = Dictionary<string,string list>
    type BestEntry = {Best:string;Resp:string;Eval:int}