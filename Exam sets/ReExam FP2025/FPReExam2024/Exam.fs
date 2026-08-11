module Exam2024

open System    
    
(* If you are importing this into F# interactive then comment out
   the line above and remove the comment for the line bellow.

   Do note that the project will not compile if you do this, but 
   it does allow you to work in interactive mode and you can just remove the '=' 
   to make the project compile again.

   You will also need to load JParsec.fs. Do this by typing
   #load "JParsec.fs" 
   in the interactive environment. You may need the entire path.

   Do not remove the module declaration (even though that does work) because you may inadvertently
   introduce indentation errors in your code that may be hard to find when switching back to project mode.

   Alternative, keep the module declaration as is, but load ExamInteractive.fsx into the interactive environment
   *)
(*
 module Exam2024 = 
 *)

(* 1: Transactions *)
    
    type shape =
        | Rectangle of float * float
        | Circle of float
        | Triangle of float * float
        
    type shapeList =
        | Empty
        | AddShape of shape * shape * shapeList
        
    let area (s: shape) : float = 
        match s with
        | Rectangle (w, h) -> w * h
        | Circle (r) -> r * r * System.Math.PI
        | Triangle (b, h) -> (b * h) / 2.0

    let circumference (s: shape) : float =       
        match s with
        | Rectangle (w, h) -> w + w + h + h
        | Circle (r) -> 2.0 * r * System.Math.PI
        | Triangle (b, h) -> b + h + System.Math.Sqrt (b * b + h * h)

    let rec totalArea (sl: shapeList) : float = 
        match sl with
        | Empty -> 0.0
        | AddShape (s1, s2, l) -> area s1 + area s2 + totalArea l
        
    let totalCircumference (sl: shapeList) =
        let rec aux acc sl =
            match sl with
            | Empty -> acc
            | AddShape (s1, s2, l) ->
                (circumference s1 + circumference s2 + acc) l
        aux 0.0 sl     
        
    let shapeListFold (f: 'a -> shape -> 'a) (acc: 'a) (sl: shapeList) = 
        List.fold f acc sl (*???*)

    let isCircle =
        function
        | Circle _ -> true
        | _        -> false

    let containsCircle trs = 
        shapeListFold (fun acc c -> acc || isCircle c) false trs
        
    let totalArea2 (sl: shapeList) = failwith "idk"

    let totalCircumference2 _ = failwith "not implemented"
    
(* 2: Code Comprehension *)
        
    let foo =
        function
        | c when Char.IsWhiteSpace c -> c 
        | c when c > 'w'             -> char (int c - 23)
        | c when c < 'x'             -> char (int c + 3)
        
    let bar (str : string) = [for c in str -> c]
    
    let baz str =
        let rec aux = 
            function
            | [] -> ""
            | c :: cs -> string (foo c) + (aux cs)
            
        aux (bar str)
    
(* Question 2.1 *)

    (* 
    
    Q: What are the types of functions foo, bar, and baz?

    A: foo: char -> char
       bar: string -> list<char>
       baz: string -> string

    Q: What do the function foo, bar, and baz do.
       Focus on what they do rather than how they do it.

    A: foo offsets every char by 3 indices, example:
        a, b and c become d, e and f, respectively
        
        bar turns a string into a list of chars, example
        "Hello" becomes ['H'; 'e'; 'l'; 'l'; 'o']

        baz encodes a string using foo on each character.
        Example: "Hello" becomes "Khoor". This is also called a Caesar cipher:)
    
    Q: What would be appropriate names for functions 
       foo, bar, and baz?

    A: foo: offsetBy3
       bar: stringToCharList
       baz: encode
        
*)
    
(* Question 2.2 *)

    (*
        We get the warning because, while every case is technically
        covered, using the word "when" for every case makes f# think
        "what about if that's NOT the case?"
        So I've added a catch-all case, which will never be reached, but
        it will calm the compiler.
    *)
    let foo2  = 
        function
        | c when Char.IsWhiteSpace c -> c 
        | c when c > 'w'             -> char (int c - 23)
        | c when c < 'x'             -> char (int c + 3)
        | c -> c

(* Question 2.3 *)
    
    let baz2 (s: string) : string = 
        s
        |> bar
        |> List.map foo
        |> List.fold (fun c acc-> string c + string acc) ""
    
(* Question 2.4 *)

    (*

    Q: The function `baz` from Question 2.1 is not tail recursive. Demonstrate why.
       To make a compelling argument you should evaluate a function call of the function,
       similarly to what is done in Chapter 1.4 of HR, and reason about that evaluation. 
       You need to make clear what aspects of the evaluation tell you that the function 
       is not tail recursive. Keep in mind that all steps in an evaluation chain must 
       evaluate to the same value ( (5 + 4) * 3 --> 9 * 3 --> 27 , for instance).
       
       You do not have to step through the foo- or the bar functions. 
       You are allowed to evaluate these function immediately.
       
    A: baz "Hey"
       -> | string (foo 'h') +       aux "ey"
         -> char (int 'h' - 23)      -> | string (foo 'e')      +  aux "y"
         -> 'k'                      -> char (int 'e' - 23)     -> | string (foo 'y')    + aux []
                                     -> 'h'                     -> char (int 'y' - 23)  -> | ""
                                                                -> 'b'
       -> 'k' + 'h' + 'b' + ""

    *)
    
(* Question 2.5 *)
    
    let bazTail (s: string) = 
        let rec aux k chars =
            match chars with
            | [] -> k ""
            | head :: tail ->
                aux (fun result ->
                    k (string (foo head) + result)) tail
        aux id (bar s)
        
(* 3: Atbash Ciphers *)

(* Question 3.1 *)
    
    let encrypt (text: string) = 
        let cl = [for c in text -> c]
        let encode c = 
            //if Char.IsWhiteSpace c then ' '
            if c = ' ' then ' '
            else
                char (122 - (int c - 97))
        let rec aux xs = 
            match xs with
            | [] -> ""
            | head :: tail -> string (encode head) + aux tail
        aux cl
    
(* Question 3.2 *)

    let decrypt (text: string) = 
        let cl = [for c in text -> c]
        let encode c = 
                //if Char.IsWhiteSpace c then ' '
                if c = ' ' then ' '
                else
                    char (122 - (int c - 97))
        let rec aux xs = 
                match xs with
                | [] -> ""
                | head :: tail -> string (encode head) + aux tail
        aux cl
    
(* Question 3.3 *)

    let rec splitAt (i: int) (str: string) : string list = 
        match str with
        | "" -> []
        | _ -> str[..(i-1)] :: splitAt i str[i..]
    //splitAt 2 "Hello world";; 

(* Question 3.4 *)
    
    let parEncrypt (str: string) (i: int) : string =
        let rec chunks s =
            if s = "" then []
            else 
                let chunks = splitAt i str
                chunks
        
        chunks str
        |> List.map (fun chunk -> 
            async {
                return encrypt chunk
            })

        |> Async.Parallel
        |> Async.RunSynchronously
        |> String.concat ""
        
    
(* Question 3.5 *)
        
    open JParsec.TextParser

    let parseEncrypt _ = failwith "not implemented"

(* 4: Letterboxes *)
    
(* Question 4.1 *)
    
    type clicker = unit // insert your own type here
    
    let newClicker _ = failwith "not implemented"

(* Question 4.2 *)
    
    let click _ = failwith "not implemented"
        
    let read _ = failwith "not implemented"

    
(* Question 4.3 *)
    type StateMonad<'a> = SM of (clicker -> 'a * clicker)  
      
    let ret x = SM (fun cl -> (x, cl))
    
    let bind f (SM a) : StateMonad<'b> =
        SM (fun cl ->
               let x, cl'  = a cl
               let (SM g) = f x
               g cl')
          
    let (>>=) x f = bind f x  
    let (>>>=) x y = x >>= (fun _ -> y)  
      
    let evalSM cl (SM f) = f cl
    
    let click2 _ = failwith "not implemented"
    
    let read2 _ = failwith "not implemented"

(* Question 4.4 *)
    
    let multipleClicks _ = failwith "not implemented"
        
(* Question 4.5 *)

    type StateBuilder() =

        member this.Bind(f, x)    = bind x f
        member this.Return(x)     = ret x
        member this.ReturnFrom(x) = x
        member this.Combine(a, b) = a >>= (fun _ -> b)

    let state = StateBuilder()
    
    let multipleClicks2 _ = failwith "not implemented"