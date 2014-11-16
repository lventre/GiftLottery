namespace GitHub.Lventre.GitLottery

open System

[<AutoOpen>]
module Helper =
    
    let private cprint writer c fmt =
        Printf.kprintf
            (fun s ->
                let orig = Console.BackgroundColor
                Console.BackgroundColor <- c
                writer(s)
                Console.BackgroundColor <- orig)
            fmt

    let cprintf c fmt = cprint Console.Write c fmt

    let cprintfn c fmt = cprint Console.WriteLine c fmt
