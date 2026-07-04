module Assignment2

//GREEN

    let rec downto1 (n: int) : int list = 
        if n = 0 then []
        else n :: downto1 (n-1) //args NEED to be parenthesized
    
    let rec downto2 (n: int) : int list = 
        match n with
        | 0 -> []
        | _ -> n :: downto2 (n-1)

    let rec downto3 = //when using function pattern match, just don't declare arguments
        function
        | 0 -> []
        | n -> n :: downto3 (n-1)

    let rec removeOddIdx xs = 
        match xs with
        | [] -> []
        | [x] -> [x]
        | y :: _ :: rest -> y :: removeOddIdx rest

    let rec combinePair (xs: 'a list) : ('a * 'a) list =
        match xs with
        | [a; b] -> [(a, b)]
        | [a; b; c] -> [(a, b)]
        | a :: b :: rest -> (a, b) :: combinePair rest

    type complex = (float * float)

    let mkComplex (a: float) (b: float) : complex = (a, b)

    let complexToPair (c: complex) : (float * float) = c

    let (|+|) (c1: complex) (c2: complex): complex = 
        match c1 c2 with
        | (a, b) (c, d) -> (a + c, b + d)

    let (|*|) (a: complex) (b: complex) = 
        let (a, b) = complexToPair a
        let (c, d) = complexToPair b
        mkComplex (a * c - b * d) (b * c + a * d)

    let (|-|) (a: complex) (b: complex) =
        let (a, b) = complexToPair a
        let (c, d) = complexToPair b
        mkComplex (a - c) (b - d)
        
    let (|/|) (a: complex) (b: complex) =
        let (a, b) = complexToPair a
        let (c, d) = complexToPair b
        let d = b * d + a * c
        mkComplex ((x1 * x2 + y1 * y2) / d)((y1 * x2 - x1 * y2) / d)

    let explode1 (s: string) = s.ToCharArray() |> List.ofArray

    let rec explode2 (s: string) = 
        if (s = "") then []
        else s.[0] :: explode2 (s.Substring(1))

    let rec implode (cs: char list): string = 
        match cs with
        | [] -> ""
        | head :: tail -> head.ToString() + implode (tail)

    let rec implodeRev (cs: char list): string =
            match cs with
            | [] -> ""
            | head :: tail -> implodeRev (tail) + head.ToString()   
   
    (*Alternatively:

    let rec implodeRev (cs: char list) = 
        match cs with
        | [] -> ""
        | _ -> cs[cs.Length-1].ToString() + implodeRev (cs.[..cs.Length - 2])
        
    *)

    let toUpper s =
        let rec uppercase (cs: char list) =
            match cs with
            | [] -> []
            | head :: tail -> System.Char.ToUpper (head) :: uppercase tail
        s |> explode2 |> uppercase |> implode

    //helper func
    let rec uppercase (cs: char list) =
            match cs with
            | [] -> []
            | head :: tail -> System.Char.ToUpper (head) :: uppercase tail

    //let toUpper2 = explode2 s >> uppercase >> implode

    let rec ack ((m, n) : (int, int)) : int =
        if (m = 0) then n + 1
        else if n = 0 then ack (m-1, 1)
            else ack (m-1, ack(m, n-1))
            
    //YELLOW
    let reverse _ = failwith "not implemented"
    let palindrome _ = failwith "not implemented"
    let keepLetters _ = failwith "not implemented"
    let palindrome2 _ = failwith "not implemented"
    
    //RED
    let palindrome3 _ = failwith "not implemented"