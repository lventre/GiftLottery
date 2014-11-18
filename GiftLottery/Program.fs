namespace GitHub.Lventre.GitLottery

open FSharp.Data
open System

module Program =

    /// Read the participants list from the CSV file
    let participants =
        cprintfn ConsoleColor.DarkYellow "Reading the participants list..."
        let csv = new CsvProvider<"Participants.csv", ";", HasHeaders = true>()
        [ for r in csv.Rows do
            yield { Email = r.Email.Trim(); Name = r.Name.Trim() } ]
        |> Seq.toArray
        
    [<EntryPoint>]
    let main argv = 
        cprintfn ConsoleColor.DarkYellow "Shuffling..."
        let shuffled = Shuffle.shuffle participants

        cprintfn ConsoleColor.DarkYellow "Sending the results..."
        use email = new Email()
        Array.iter2 (fun itm1 itm2 -> email.Send (itm1, itm2)) participants shuffled

        cprintfn ConsoleColor.DarkYellow "Done!"
        
        cprintfn ConsoleColor.Gray "\nPlease press any key to exit..."
        Console.ReadKey() |> ignore
        0