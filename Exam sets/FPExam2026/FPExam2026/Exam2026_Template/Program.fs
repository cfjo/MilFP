open Exam2026_Template.Exam


let testQ11_block () =
    [0..9] |> List.map (lucas_number 1)

//printfn "%A" (approx_steps_needed 2 0.0001)

let b = empty 3
printfn "%A" (b)
printfn "%A" (get_dimension b)
printfn "%A" (has_queen (1) (Some 1) (b))
printfn "%A" ()
printfn "%A" ()
printfn "%A" ()
printfn "%A" ()

//1 |> lucas_seq |> Seq.take 10 |> Seq.toList |> printfn "%A"

//() |> testQ11_block |> printfn "%A"