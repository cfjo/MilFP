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
        
    let mkState () = {Variables = Map.empty} //unit -> state
    
    let declare x st : state option = 
        if 
            st.Variables.ContainsKey x
            && (not (reservedVariableName x))
            && validVariableName x
        then Some { Variables = st.Add(x, 0) }

        else None

    (*

    let getVar (s: string) (st: state) : int option =
        if st.Variables.ContainsKey s
        then Some (st.Variables[s])
        else None

    *)
    
    let getVar (s: string) (st: state) : int option =
        Map.tryFind s st.Variables
        
    let setVar (x: string) (v: int) (st: state) = 
        if st.Variables.ContainsKey s
        then Some (st with Variables = Map.add x v st.Variables)
        else None
    
    //YELLOW
    let push _ = failwith "not implemented"
    let pop _ = failwith "not implemented"     