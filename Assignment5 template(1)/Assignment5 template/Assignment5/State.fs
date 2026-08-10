module Interpreter.State

    open Result
    open Language
    
    let reservedVariableName (v: string) : bool =
        let reservedNames = ["if"; "then"; "else"; "while"; "declare"; "print"; "random"; "fork"; "__result__"]
        reservedNames |> List.exists (fun x -> x = v)
   
    let validVariableName (v: string) : bool =
        (System.Char.IsAsciiLetter v[0] || v[0] = '_')
        && (v |> String.forall (fun c -> System.Char.IsAsciiLetterOrDigit c || c = '_'))

    type state = 
    {
       Variables : Map<string, int>
    }
        
    let mkState () = state {Variables = Map.empty} //unit -> state
    
    let declare _ = failwith "not implemented"
    
    let getVar _ = failwith "not implemented"
    let setVar _ = failwith "not implemented"
    
    let push _ = failwith "not implemented"
    let pop _ = failwith "not implemented"     