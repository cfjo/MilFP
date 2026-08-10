module Interpreter.Eval

    open Result
    open Language
    open State
    
    let rec aexprEval (a: aexpr) (st: state) : int option = 
    
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

    let aexprEval2 _ = failwith "not implemented"
    
    let bexprEval _ = failwith "not implemented"
    let stmntEval _ = failwith "not implemented"