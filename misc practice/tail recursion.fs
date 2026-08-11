    let rec sum (l: int list) (acc: int) =
        match l with
        | [] -> acc
        | head :: tail -> acc + head + sum tail acc

    let sum2 (l: int list) =
        let rec aux (ls: int list) (acc: int) =
            match ls with
            | [] -> acc
            | head :: tail -> acc + head + aux tail acc
        aux l 0
    //sum2 [1;2;3;4];;

    (* ^^^^^ These are NOT tail recursive. The recursive call is not the last thing that happens.
    After it returns, F# still has to compute acc + head + (result) 
    So the compiler has to keep the current stack frame around until aux tail acc finishes, meaning it isn't tail recursive.*)
    
    (*-----------------------------------------------------------*)

    let sumA lst =
        let rec aux (l: int list) (acc: int)
            match l with
            | [] -> acc
            | head :: tail -> aux tail (acc + head)
        aux lst 0

    let length (lst: 'a list) =
        let rec aux (l: 'a list) (acc: int) =
            match l with
            | [] -> acc
            | head :: tail -> aux tail (acc + 1)
        aux lst 0

    let rev (lst: 'a list) =
        let rec aux (xs: 'a list) (acc: 'a list) =
            match xs with
            | [] -> acc
            | head :: tail -> aux tail (head :: acc)
        aux lst [] 

    let fact (n: int) =
        let rec aux n acc =
            if n <= 1 then acc
            else aux (n - 1) (acc * n)
        aux n 1

    let factorial n =
        let rec loop n acc =
            if n <= 1 then
                acc
            else
                loop (n - 1) (acc * n)
        loop n 1

    let pow (b: int) (exponent: int) =
        let rec aux b exp acc =
            match exp with
            | 0 -> acc
            | _ -> aux b (exp - 1) (acc * b)
        aux b exponent 1

    let count (what: 'a) (where: 'a list) : int =
        let rec aux (xs: 'a list) (acc: int) =
            match xs with
            | [] -> acc
            | head :: tail -> 
                if head = what then
                    aux tail acc + 1
                else aux tail acc
        aux where 0

    let max (lst: int list) : int =
        let rec aux (xs: int list) (acc: int) =
            match xs with
            | [] -> acc
            | head :: tail ->
                if head > acc then 
                    aux tail head
                else
                    aux tail acc
        aux lst 0
    
    let fib n =
        let rec loop i fibI fibNext =
            if i = n then
                fibI
            else
                loop (i + 1) fibPrev (fibI + fibNext)
        loop 0 0 1

    (*

    fibI    = fib (i) 
    fibNext = fib (i + 1)
    
    *)