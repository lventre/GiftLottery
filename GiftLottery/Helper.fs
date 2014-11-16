namespace GitHub.Lventre.GitLottery

open System

[<AutoOpen>]
module Helper =
    
    let private cprint writer c =
        Printf.kprintf
            (fun s ->
                let orig = Console.BackgroundColor
                Console.BackgroundColor <- c
                writer(s)
                Console.BackgroundColor <- orig)

    /// <summary>
    /// Prints on the console, without line return.
    /// </summary>
    /// <param name="c">Console color.</param>
    /// <param name="fmt">String format.</param>
    let cprintf c fmt = cprint Console.Write c fmt

    /// <summary>
    /// Prints on the console, followed with a line return character.
    /// </summary>
    /// <param name="c">Console color.</param>
    /// <param name="fmt">String format.</param>
    let cprintfn c fmt = cprint Console.WriteLine c fmt
