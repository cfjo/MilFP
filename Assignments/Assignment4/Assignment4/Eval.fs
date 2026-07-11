module Interpreter.Eval

open Result
open Language
    
let rec aexprToString a = 
    match a with
    | Num n -> string n
    | Add (e1, e2) -> "(" + aexprToString e1 + "+" + aexprToString e2 + ")"
    | Mul (e1, e2) -> "(" + aexprToString e1 + "*" + aexprToString e2 + ")"
    | Div (e1, e2) -> "(" + aexprToString e1 + "/" + aexprToString e2 + ")"
            
    (*Hvis man bruger function skal man ikke erklære argumentet i signaturen*)
    
    (* Alternatively 

    let rec aexprToString =
    function
    | Num n -> string n
    | Add (e1, e2) ->
        "(" + aexprToString e1 + "+" + aexprToString e2 + ")"
    | Mul (e1, e2) ->
        "(" + aexprToString e1 + "*" + aexprToString e2 + ")"
    | Div (e1, e2) ->
        "(" + aexprToString e1 + "/" + aexprToString e2 + ")"
    
    *)

let rec bexprToString b = 
    match b with
    | TT -> "true"
    | Eq (e1, e2) -> "(" + aexprToString e1 + " = " + aexprToString e2 + ")"
    | Lt (e1, e2) -> "(" + aexprToString e1 + " < " + aexprToString e2 + ")"
    | Conj (e1, e2) -> "(" + bexprToString e1 + " /\ " + bexprToString e2 + ")"
    | Not e -> "( not " + bexprToString e + ")"

let rec aexprEval (a: aexpr) : int option = 
    match a with
    | Num x -> Some x
    | Add (b, c) ->
        match aexprEval b, aexprEval c with
        | Some x, Some y -> Some (x + y)
        | _ -> None
    | Mul (b, c) ->
        match aexprEval b, aexprEval c with
        | Some x, Some y -> Some (x * y)
        | _ -> None
    | Div (b, c) ->
        match aexprEval b, aexprEval c with
        | Some x, Some 0 -> None
        | Some x, Some y -> Some (x / y)
        | _ -> None

let rec aexprEval2 a = 
    match a with
    | Num x -> Some x
    | Add (b, c) -> aexprEval2 b 
                    |> Option.bind (fun x ->
                        aexprEval2 c 
                        |> Option.bind (fun y ->
                             Some (x + y)))
    | Mul (b, c) -> aexprEval2 b
                    |> Option.bind (fun x -> aexprEval2 c |> Option.bind (fun y -> Some (x * y)))
    | Div (b, c) ->
                 aexprEval2 b
                    |> Option.bind (fun x ->
                        aexprEval2 c
                        |> Option.bind (fun y ->
                            if y = 0 then
                                None
                            else
                                Some (x / y )))

let rec bexprEval (b: bexpr) : bool option =
    match b with
    | TT -> Some true
    | Eq (a, c) -> 
        match aexprEval a, aexprEval c with
        | Some x, Some y -> Some (x = y)
        | _ -> None
    | Lt (a, c) -> 
        match aexprEval a, aexprEval c with
        | Some x, Some y -> Some (x > y)
        | _ -> None
    | Conj (a, c) -> 
        match bexprEval a, bexprEval c with
        | Some x, Some y -> Some (x && y)
        | _ -> None
    | Not a ->
        match bexprEval a with
        | Some x -> Some (not x)
        | _ -> None

let aexprToString2 a = 
    let rec aux (a: aexpr) : int * string =
     match a with
     | Num n -> (0, string n) (*numbers live on level 0*)
     | Add (b, c) ->
        let (l1, s1) = aux b
        let (l2, s2) = aux c
        let level = 2 (*additions live on level 2*)

        let left = if level < l1 then "(" + s1 + ")" else s1 (*if b *)
        let right = if level < l2 then "(" + s2 + ")" else s2
        level, left + " + " + right

     | Mul (b, c) -> 
        let (l1, s1) = aux b
        let (l2, s2) = aux c
        let level = 1 (*multiplication lives on level 1*)

        let left = if level < l1 then "(" + s1 + ")" else s1
        let right = if level <l2 then "(" + s2 + ")" else s2
        level, left + " * " + right

     | Div (b, c) -> 
        let (l1, s1) = aux b
        let (l2, s2) = aux c
        let level = 1
        
        let left = if level < l1 then "(" + s1 + ")" else s1
        let right = if level <= l2 then "(" + s2 + ")" else s2
        level, left + " / " + right
    
    let (_, s) = aux a (*only keeping the string*)
    s

(*
TT lives on level 0
not lives on level 1
equality and less than lives on level 2
conjunction lives on level 3
*)

let bexprToString2 b =
    let rec aux b : int * string =
        match b with
        | TT -> (0, "true")
        | Eq (a, c) -> 
            let s1 = aexprToString a
            let s2 = aexprToString c
            (2, s1 + " = " + s2)

        | Lt (a, c) ->
            let s1 = aexprToString a
            let s2 = aexprToString c
            (2, s1 + " < " + s2)
            
        | Conj (a, c) ->
            let (l1, s1) = aux a
            let (l2, s2) = aux c
            let level = 3

            let left = if level < l1 then "(" + s1 + ")" else s1
            let right = if level <l2 then "(" + s2 + ")" else s2

            level, left + " /\\ " + right

        | Not a ->
            let (l, s) = aux a
            let level = 1

            let inner = if level <= l then "(" + s + ")" else s
            level, "not" + inner
    
    let (_, s) = aux b
    s

//YELLOW/RED
let aexprFold _ = failwith "not implemented"
let bexprFold _ = failwith "not implemented"
    
let aexprToString3 _ = failwith "not implemented"
let bexprToString3 _ = failwith "not implemented"
let aexprEval3 _ = failwith "not implemented"
let bexprEval3 _ = failwith "not implemented"
let aexprToString4 _ = failwith "not implemented"
let bexprToString4 _ = failwith "not implemented"
