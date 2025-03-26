namespace GenRepLib

open System.Collections.Generic

[<AutoOpen>]
module Types =
    type RespCacheDict = Dictionary<string,string list>
    type BestEntry = {Best:string;Resp:string;Eval:int}
    type BestCdbEntry = {Best:string;Score:int;Rank:int}
    type BestCacheDict = Dictionary<string,BestEntry>
    type BestCdbCacheDict = Dictionary<string,BestCdbEntry>
