open Exam2026_Template.Exam
open JParsec.TextParser

let testQ11_block () =
    [0..9] |> List.map (lucas_number 1)

printfn "%A" (approx_steps_needed 2 0.0001)

(*let b = empty 3
printfn "%A" (b)
printfn "%A" (get_dimension b)
printfn "%A" (has_queen 1 1 b)
let b2 = place_queen 1 2 b
printfn "%A" (b2)*)

(*Question 3.1*)
printfn "%A" (encode "Hello World!")
printfn "%A" (decode "HoHelollolo WoWororloldod!")

(*Q 3.3*)
decode "FoF# isos amomazozinongog" |> printfn "%A"
encode_fun (fun c -> let cstr = string c in cstr + cstr) "Hello World!" |> printfn "%A"

(*Q 3.4*)
run parser_robbers_language "Hello World!" |> printfn "%A"

//Question 4.2
(*
let b1 = empty 4 |> place_queen 1 0 |> Option.bind (place_queen 3 1) |> Option.bind (place_queen 0 2)
let b2 = b1 |> Option.bind (place_queen 2 3)
empty 4 |> place_queen 1 0 |> Option.bind (place_queen 0 1) |> printfn "%A"
b1 |> Option.get |> valid_solution |> printfn "%A"
b2 |> Option.get |> valid_solution |> printfn "%A"
*)

//1 |> lucas_seq |> Seq.take 10 |> Seq.toList |> printfn "%A"



//() |> testQ11_block |> printfn "%A"