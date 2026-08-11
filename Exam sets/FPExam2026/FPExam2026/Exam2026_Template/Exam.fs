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
    let lucas_number_acc (k: int) (n: int) (acc: int) =
        let rec loop n a b = 
            match n with
            | 0 -> a
            | _  -> loop (n - 1) b (k * b + a)
        loop n 0 1

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
    let lucas_seq (k: int) : seq<int> = 
        Seq.unfold (fun (a, b) -> Some (a, (b, k * b + a))) (2, k)

    (* Question 2: Code comprehension (25%) *)
    
 
    (* Question 2.1 *)
    (*
     
     Q: What do the functions `foo`, `bar`, and `baz` do? Focus on what they do rather than how they do it.
     A: foo returns the smallest divisor of x (finds the first divisor of x, starting from a)
        bar returns the smallest factor of x, aside from 1. Also makes sure not to divide by 0.
        also, if bar x = x, then x is a prime number
        baz returns the prime factorizations of x as a list 
        (aka, the prime numbers that, multiplied with each other gives x) 
        ex: baz 8 = [2, 2, 2] because 2 * 2 * 2 = 8

     Q: What would be appropriate names for functions `foo`, `bar`, and `baz`.
     A: foo = smallestDivisor
        bar = smallestFactor
        baz = primeFactors     
     
     Q: For these functions to behave meaningfully, we must place a restriction on the input values. What restriction?
     A: The input must be a nonnegative integer // the input must be an integer >= 2
    *)
    
    (* Question 2.2 *)
    
    let rec foo2 x = 
        function  
        | a when x % a = 0 -> a  
        | a -> foo2 x (a + 1)

    
    (* Question 2.3 *)
    
    let baz_inverse (x: int list) =
        List.fold (fun x acc -> acc * x) 1 x
                //function, starting value, list
    //baz_inverse [2; 2; 3]
    
    (* Question 2.4 *)
    
    (*
      Q: One of the functions from Question 2.1 is not tail recursive.
      Explain which one and why. To make a compelling argument you must evaluate
      a function call of the function, similarly to what is done in
      Chapter 1.4 of HR, and reason about that evaluation. You need to make clear
      what aspects of the evaluation tell you that the function is not tail recursive.
      Keep in mind that all steps in an evaluation chain must evaluate to the
      same value (```(5 + 4) * 3 --> 9 * 3 --> 27```, for instance).
      
      A: baz is not tail-recursive. This is because it starts with "match bar x with",
      meaning we have to wait for bar to return a result, before we can evaluate the rest
      of the expression. Bar itself also has to call another function, which needs to be
      evaluated before we can move on. 
      Example:
      baz 8 -> (bar 8 -> foo 8 2 -> 2) 
      match 2 with
      | 2 -> 2 :: (baz (x / y))
               -> (baz (2/8))
               -> (baz 4)
               -> match bar 4 (foo 4 2 -> 2) with
                    | 2 -> 2 :: 2 :: (baz 4 / 2)
                                  -> (baz 2)
                                  -> match bar 2 (foo 2 2 -> 2) with
                                     | 2 = 2 -> [2]
      -> 2 :: 2 :: 2

    *)
    
    (* Question 2.5 *)
    
    let rec foo x =
        function  
        | a when x = a     -> a  
        | a when x % a = 0 -> a  
        | a                -> foo x (a + 1)  
    let rec bar =  
        function  
        | 0 -> 0  
        | 1 -> 1  
        | x -> foo x 2
    let barC x k =
        k (bar x)
    let cont (x: int) = 

    
    (* Question 3: The robbers language (25%) *)
    
    let explode (str : string) = [for c in str -> c]  

    let implode (cs : char list) = cs |> Array.ofList |> System.String  

    let isConsonant (c : char) = "bcdfghjklmnpqrstvwxz".IndexOf(System.Char.ToLower c) >= 0
        
    (* Question 3.1 *)
    
    let encode (str: string) : string =
        let s = explode str
        let rec aux sl =
            match sl with
            | [] -> []
            | head :: tail ->
                if isConsonant head then 
                    head :: 'o' :: head :: aux tail
                else head :: aux tail
        s |> aux |> implode

    (* Question 3.2 *)
    
    let decode(str: string) : string = 
        let s = explode str
        let rec aux cl = 
            match cl with
            | []  -> []
            | head :: 'o' :: same :: tail ->
                if isConsonant head 
                    then head :: aux tail
                else head :: aux ('o' :: same :: tail) 
            | head :: tail -> head :: aux tail
        s |> aux |> implode     
    
    (* Question 3.3 *)
    //List.fold folder state list
    //state = like an accumulator

    let encode_fun (f: char -> string) (str: string) : string = 
        let cl = explode str
        List.fold (fun acc c -> acc + f c) "" cl
        
        (*
            fun acc c -> acc + f c = function
            "" = initial state
            cl = list
        *)
    
    let encode2 (str: string) =
        encode_fun 
            (fun c ->
                if isConsonant c then
                    string c + "o" + string c
                else
                    string c
            ) str

    (* Question 3.4 *)
        
    let parser_robbers_language = 
        many anyChar |>> implode |>> encode
    
    (* Question 3.5 *)
    
    let encode_par (str: string) (num: int) =
        //make a list of words back into a single space-seperated string
        let composeWords (words: string list) : string =
            String.concat " " words
        
        let words = str.Split(' ') |> Array.toList

        let n = List.length words
        
        if n = 0 then ""
        else
            (*celing division: number of words per chunk, so we only get at most 'num' chunks*)
            let chunkSize = (n + num - 1) / num
            let chunks = List.chunkBySize chunkSize words

            //spawn one task per chunk; each task encodes it's own words and composes them
            let tasks =
                chunks
                |> List.map (fun chunk ->
                    System.Threading.Tasks.Task.Run(fun () ->
                        chunk |> List.map encode |> composeWords))
            
            //wait for all tasks and collect their results in order
            let results = tasks |> List.map (fun t -> t.Result)
 
            composeWords results

    (* Question 4: The N-Queens problem (25%) *)
     

    (* Question 4.1 *)
    
    type board = 
        {
            n: int;
            rows: int list;
            (* liste der svarer til rækker, indeholder hvor på rækken,
            der er placeret et dronning eks:
            [1, 0, 2] har en dronning på række 0, kollone 1
            -1 = no queen *)
        }
            
    let rec makeList n : int list =
        if n = 0 then []
        else -1 :: makeList (n - 1)
    
    let empty (N: int) : board =
     { n = N
       rows = makeList N }
    
    let get_dimension (b: board) = b.n
    
    let has_queen (r: int) (c: int) (b: board) = 
        if b.rows[r] = c then true else false
    
    (* Question 4.2 *)
        
    let place_queen (r: int) (c: int) (b: board) : board option = 
        let rec checkColumn row =
                if row = -1 then true else
                    if b.rows[row] <> c then
                        checkColumn (row - 1)
                    else false
        
        let rec replaceSquare (row: int) (column: int) (xs: int list) : int list =
            match xs with
            | [] -> []
            | head :: tail ->
                if row = 0 then
                    column :: tail
                else
                    head :: replaceSquare (row - 1) column tail

        if b.rows[r] = -1 && checkColumn (b.n - 1) then
            Some {b with rows = replaceSquare r c b.rows}
        else None

        (*1. tjek om der er noget på rækken
          2. tjek om der er noget i kolonnen
          3. hvis ja -> None
          4. hvis nej -> b.rows[r] = c*)
        
    let valid_solution (b: board) : bool = 
        let rec countQueens (l: int list) (acc: int) =
            match l with
            | [] -> acc
            | head :: tail ->
                if head <> -1 then countQueens tail (acc + 1)
                else acc
        countQueens b.rows 0 = b.n
    
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