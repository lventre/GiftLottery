namespace GitHub.Lventre.GitLottery

open FSharp.Data
open System

module Program =

    /// Read the participants list from the CSV file
    let readParticipants =
        let csv = new CsvProvider<"Participants.csv", ";", HasHeaders = true>()
        [ for r in csv.Rows do
            yield { Email = r.Email.Trim(); Name = r.Name.Trim() } ]
        |> Seq.toArray
        
    [<EntryPoint>]
    let main argv = 
        let participants = readParticipants
        printfn "%A" participants

        let shuffled = Shuffle.shuffle participants

        cprintfn ConsoleColor.DarkGreen "Shuffling..."
        printfn "%A" shuffled

        cprintfn ConsoleColor.DarkGreen "Sending the results..."
        Array.iter2 (fun itm1 itm2 -> Email.send itm1 itm2) participants shuffled

        cprintfn ConsoleColor.DarkGreen "Done..."
        0