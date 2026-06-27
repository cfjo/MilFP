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

    let (|*|) _ = failwith "not implemented"
    let (|-|) _ = failwith "not implemented"
    let (|/|) _ = failwith "not implemented"

    let explode1 _ = failwith "not implemented"

    let rec explode2 _ = failwith "not implemented"

    let implode _ = failwith "not implemented"
    let implodeRev _ = failwith "not implemented"

    let toUpper _ = failwith "not implemented"

    let ack _ = failwith "not implemented"
    
    //YELLOW
    let reverse _ = failwith "not implemented"
    let palindrome _ = failwith "not implemented"
    let keepLetters _ = failwith "not implemented"
    let palindrome2 _ = failwith "not implemented"
    
    //RED
    let palindrome3 _ = failwith "not implemented"