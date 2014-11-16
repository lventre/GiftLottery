namespace GitHub.Lventre.GitLottery

open FSharp.Data
open System

module Program =

    /// Read the participants list from the CSV file
    let readParticipants csvFilePath =
        let csv = new CsvProvider<"Participants.csv", ";", HasHeaders = true>()
        [ for r in csv.Rows do
            yield { Email = r.Email.Trim(); Name = r.Name.Trim() } ]
        |> Seq.toArray
        
    [<EntryPoint>]
    let main argv = 
        let participants = readParticipants ()
        printfn "%A" participants

        let shuffled = Shuffle.shuffle participants

        cprintfn ConsoleColor.Green "Shuffling..."
        printfn "%A" shuffled

        0