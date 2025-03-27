namespace KindleChess

open System.IO
open DotLiquid
open Microsoft.FSharp.Reflection
open FSharp.Formatting.Markdown

module Book =
    
    ///genh - generates HTML files for book
    let genh title fls pfol tfol ifol ofol =
        try 
            let gms = 
                fls
                |>Array.map(Pgn.ReadGames)
                |>Array.map(fun gml -> gml.Head)
            Directory.CreateDirectory(ofol)|>ignore
            //do label for games
            let glbls = gms|>Array.map(fun gm -> (if (gm.WhitePlayer<>"") && (gm.WhitePlayer<>"?") then gm.WhitePlayer else gm.BlackPlayer))

            //write files
            //register types used
            let reg ty =
                let fields = FSharpType.GetRecordFields(ty)
                Template.RegisterSafeType(ty, [| for f in fields -> f.Name |])
            reg typeof<Game>
            //Template.RegisterFilter typeof<TreeT>
            //create output
            //do simple options
            let gendoc inp oup =
                let t =
                    Path.Combine(tfol, inp)
                    |> File.ReadAllText
                    |> Template.Parse
                
                let ostr =
                    t.Render(Hash.FromDictionary(dict [ "title", box title; "glbls", box glbls ]))
                let ouf = Path.Combine(ofol, oup)
                File.WriteAllText(ouf, ostr)
            gendoc "opf.dotl" "book.opf"
            gendoc "ncx.dotl" "book.ncx"
            gendoc "toc.dotl" "toc.html"
            //copy standard files
            let css = Path.Combine(tfol, "book.css")
            let ocss = Path.Combine(ofol, "book.css")
            File.Copy(css, ocss, true)
            let gif = Path.Combine(tfol, "cover.gif")
            let ogif = Path.Combine(ofol, "cover.gif")
            File.Copy(gif, ogif, true)
            //do welcome
            let t =
                Path.Combine(tfol, "Welcome.dotl")
                |> File.ReadAllText
                |> Template.Parse
            
            let wel =
                let welfl = Path.Combine(pfol, title + ".md")
                File.ReadAllText(welfl)
                |> Markdown.Parse
                |> Markdown.ToHtml
            
            let ostr = t.Render(Hash.FromDictionary(dict [ "wel", box wel ]))
            let ouf = Path.Combine(ofol, "Welcome.html")
            File.WriteAllText(ouf, ostr)
            // variations
            let t =
                Path.Combine(tfol, "Variations.dotl")
                |> File.ReadAllText
                |> Template.Parse
            
            let vars =
                gms |> Array.mapi (fun i c -> c |> Chap.ToVar i)
            let ostr = t.Render(Hash.FromDictionary(dict [ "vars", box vars; "glbls", box glbls]))
            let ouf = Path.Combine(ofol, "Variations.html")
            File.WriteAllText(ouf, ostr)
            // chapters
            gms |> Array.iteri (Chap.genh tfol ifol ofol)
        with e -> failwith ("Generation failed with error: " + e.ToString())
    
    ///genhb - generates HTML files for book given one file with many pgns
    let genhb title fl pfol tfol ifol ofol =
        try 
            let gms = fl|>Pgn.ReadGames|>List.toArray
            Directory.CreateDirectory(ofol)|>ignore
            //do label for games
            let glbls = gms|>Array.map(fun gm -> (if (gm.WhitePlayer<>"") && (gm.WhitePlayer<>"?") then gm.WhitePlayer else gm.BlackPlayer))

            //write files
            //register types used
            let reg ty =
                let fields = FSharpType.GetRecordFields(ty)
                Template.RegisterSafeType(ty, [| for f in fields -> f.Name |])
            reg typeof<Game>
            //Template.RegisterFilter typeof<TreeT>
            //create output
            //do simple options
            let gendoc inp oup =
                let t =
                    Path.Combine(tfol, inp)
                    |> File.ReadAllText
                    |> Template.Parse
                
                let ostr =
                    t.Render(Hash.FromDictionary(dict [ "title", box title; "glbls", box glbls ]))
                let ouf = Path.Combine(ofol, oup)
                File.WriteAllText(ouf, ostr)
            gendoc "opf.dotl" "book.opf"
            gendoc "ncx.dotl" "book.ncx"
            gendoc "toc.dotl" "toc.html"
            //copy standard files
            let css = Path.Combine(tfol, "book.css")
            let ocss = Path.Combine(ofol, "book.css")
            File.Copy(css, ocss, true)
            let gif = Path.Combine(tfol, "cover.gif")
            let ogif = Path.Combine(ofol, "cover.gif")
            File.Copy(gif, ogif, true)
            //do welcome
            let t =
                Path.Combine(tfol, "Welcome.dotl")
                |> File.ReadAllText
                |> Template.Parse
            
            let wel =
                let welfl = Path.Combine(pfol, title + ".md")
                File.ReadAllText(welfl)
                |> Markdown.Parse
                |> Markdown.ToHtml
            
            let ostr = t.Render(Hash.FromDictionary(dict [ "wel", box wel ]))
            let ouf = Path.Combine(ofol, "Welcome.html")
            File.WriteAllText(ouf, ostr)
            // variations
            let t =
                Path.Combine(tfol, "Variations.dotl")
                |> File.ReadAllText
                |> Template.Parse
            
            let vars =
                gms |> Array.mapi (fun i c -> c |> Chap.ToVar i)
            let ostr = t.Render(Hash.FromDictionary(dict [ "vars", box vars; "glbls", box glbls]))
            let ouf = Path.Combine(ofol, "Variations.html")
            File.WriteAllText(ouf, ostr)
            // chapters
            gms |> Array.iteri (Chap.genh tfol ifol ofol)
        with e -> failwith ("Generation failed with error: " + e.ToString())
    
