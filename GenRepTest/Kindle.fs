namespace GenRepTest

open System.IO
open NUnit.Framework
open FsUnit
open GenRepLib

module Kindle =
    
    [<SetUp>]
    let Setup () =
        Rep.fol <- @"D:\Github\GenRep\Rep"
        Kindle.tfol <- @"D:\Github\GenRep\Templates"
        Kindle.ifol <- @"D:\Github\GenRep\Images"
        ()

    [<Test>]
    let WhiteOpf() =
        Kindle.WhiteOpf "GrobWhite"
        let fls = Directory.GetFiles(Path.Combine(Rep.wfol(),"Kindle"),"*.html")
        fls.Length|>should equal 5

    [<Test>]
    let BlackOpf() =
        Kindle.BlackOpf "GrobBlack"
        let fls = Directory.GetFiles(Path.Combine(Rep.bfol(),"Kindle"),"*.html")
        fls.Length|>should equal 4

        
