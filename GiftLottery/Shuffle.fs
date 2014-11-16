namespace GitHub.Lventre.GitLottery

open System

module Shuffle =

    /// Knuth shuffle algorithm
    let private knuthShuffle (lst:array<'a>) =
        let Swap i j =                                              // Standard swap
            let item = lst.[i]
            lst.[i] <- lst.[j]
            lst.[j] <- item
        let rnd = new Random()
        let ln = lst.Length
        [0..(ln - 2)]                                               // For all indices except the last
        |> Seq.iter (fun i -> Swap i (rnd.Next(i, ln)))             // swap th item at the index with a random one following it (or itself)
        lst                                                         // Return the list shuffled in place

    let private ensureShuffled (orig:array<'a>) (shuffled:array<'a>) =
        if orig.Length <> shuffled.Length then failwith "Original and shuffled arrays have different length!"
        Array.forall2 (fun o s -> o <> s) orig shuffled

    let shuffle (arr:array<'a>) =
        let rec shuffle' (oth:array<'a>) =
            let shuf = knuthShuffle oth
            if not <| ensureShuffled oth shuf then shuffle' oth
            else shuf
        shuffle' arr