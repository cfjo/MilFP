module Exam2026_Template.Exam

    open JParsec.TextParser

    (* Question 1: Parametric Lucas numbers (25%) *)
    
    (* Question 1.1 *)
    let rec lucas_number (k: int) (n: int) : int = 
        match n with
        | 0 -> 0
        | 1 -> 1
        | _ -> k * (lucas_number k (n - 1)) + lucas_number k (n - 2)
    
    (* Question 1.2 *)
    let lucas_number_acc (k: int) (n: int) (acc: int) = failwith "nuh uh"
        //let rec aux i cur next
    (*I'll get back to you*)

    (* Question 1.3 *)
    let sqrt_approx (k: int) (n: int) : float =
        float 2 * (float (lucas_number k n) / float (lucas_number k (n - 1))) - float k

    (* Question 1.4 *)
    let approx_steps_needed (k: int) (epsilon: float) =
        let rec check n = 
            let sq = sqrt(float (k * k + 4)) - (sqrt_approx k n)
            if abs sq < epsilon then n
            else check (n + 1)
        check 2

    (* Question 1.5 *)
    let lucas_seq (k: int) : seq<int> = failwith "not implemented"
        
        


    (* Question 2: Code comprehension (25%) *)
    
 
    (* Question 2.1 *)
    (*
     
     Q: What do the functions `foo`, `bar`, and `baz` do? Focus on what they do rather than how they do it.
     A: <Your answer goes here>     
      
     Q: What would be appropriate names for functions `foo`, `bar`, and `baz`.
     A: <Your answer goes here>     
     
     Q: For these functions to behave meaningfully, we must place a restriction on the input values. What restriction?
     A: <Your answer goes here>
    *)
    
    (* Question 2.2 *)
    
    let foo2 _ = failwith "not implemented"
    
    (* Question 2.3 *)
    
    let baz_inverse _ = failwith "not implemented"
    
    (* Question 2.4 *)
    
    (*
      Q: One of the functions from Question 2.1 is not tail recursive.
      Explain which one and why. To make a compelling argument you must evaluate
      a function call of the function, similarly to what is done in
      Chapter 1.4 of HR, and reason about that evaluation. You need to make clear
      what aspects of the evaluation tell you that the function is not tail recursive.
      Keep in mind that all steps in an evaluation chain must evaluate to the
      same value (```(5 + 4) * 3 --> 9 * 3 --> 27```, for instance).
      
      A: <Your answer goes here>
    *)
    
    (* Question 2.5 *)
    
    let cont _ = failwith "not implemented"
    
    (* Question 3: The robbers language (25%) *)
    
    let explode (str : string) = [for c in str -> c]  

    let implode (cs : char list) = cs |> Array.ofList |> System.String  

    let isConsonant (c : char) = "bcdfghjklmnpqrstvwxz".IndexOf(System.Char.ToLower c) >= 0
        
    (* Question 3.1 *)
    
    let encode _ = failwith "not implemented"

    (* Question 3.2 *)
    
    let decode _ = failwith "not implemented"

    (* Question 3.3 *)
    
    let encode_fun _ = failwith "not implemented"
        
    (* Question 3.4 *)
    
    let parser_robbers_language = pstring "not implemented"
    
    (* Question 3.5 *)
    
    let encode_par _ = failwith "not implemented"

    (* Question 4: The N-Queens problem (25%) *)
    
    (* Question 4.1 *)
    
    type board = 
        {
            n: int;
            rows: option<int> list;
            (* liste der svarer til rækker, indeholder hvor på rækken,
            der er placeret et dronning eks:
            [1, 0, 2] har en dronning på række 0, kollone 1*)
        }
            
    let rec makeList n : option<int> list =
        if n = 0 then []
        else None :: makeList (n - 1)
    
    let empty (N: int) : board =
     { n = N
       rows = makeList N }
    
    let get_dimension (b: board) = b.n
    
    let has_queen (r: int) (c: int option) (b: board) = 
        if b.rows[r] = c then true else false
    
    (* Question 4.2 *)
        
    let place_queen _ = failwith "not implemented"
    
    let valid_solution _ = failwith "not implemented"
    
    (* Question 4.3 *)
    type chessMonad<'a> = CM of (board -> ('a * board) option)  

    let ret x = CM (fun h -> (Some (x, h)))    
    let fail  = CM (fun _ -> None)    
    let bind f (CM a)  =    
        CM (fun b ->    
        match a b with    
        | Some (x, b') ->    
            let (CM g) = f x    
            g b'          
        | None -> None)    

    let (>>=) a f = bind f a  
    let (>>>=) a b = a >>= (fun _ -> b)  
      
    let evalCM (CM f) N = f (empty N) 
            
    let place_queen2 _ = failwith "not implemented"
    
    let valid_solution2 = ret true // your solution goes here

    (* Question 4.4 *)
        
    let create_solution _ = failwith "not implemented"
    
    (* Question 4.5 *)
    
    type ChessBuilder() =
        member this.Bind(f, x)    = bind x f
        member this.Return(x)     = ret x
        member this.ReturnFrom(x) = x
        member this.Combine(a, b) = a >>= (fun _ -> b)

    let chess = new ChessBuilder()
    
    let create_solution2 _ = failwith "not implemented"