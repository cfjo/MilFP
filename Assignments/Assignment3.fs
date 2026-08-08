module Assignment3
    
    let add5 x = x + 5
        
    let mul3 x = x * 3  

    let add5mul3 x = x |> add5 |> mul3

    let add5mul3_2 = add5 >> mul3
    
    let add5_2 (f: 'a -> int) (x: 'a) = (f >> add5) x

    let mul3_2 (f: 'a -> int) (x: 'a) = (f << mul3) x
   
    let rec downto4 f n e = 
        match n with
        | 0 -> e
        | _ -> f n (downto4 (f (n-1) e))
                
    let fac x = downto4 (fun x -> x * x) x 1
    
    let rec range g n =
      if n > 0 then 
        [g n] @ range (g n - 1)
      else []
    
    let rec double (lst: int list) = 
      match lst with
      | [] -> []
      | head :: tail -> head * 2 :: double tail //brug :: ik @
        
    let double_2 (lst: int list) = 
      lst |> List.map (fun x -> x * 2)
    (*hvis man bare skriver List.map (etc), returneres funktionen, ikke resultatet af funktionen*)
    
    let rec stringLength (lst: string list) : int list = 
      match lst with
      | [] -> []
      | head :: tail -> head.Length :: stringLength tail
    //stringLength2 ["hi"; "hello"; "Heeeyy"];;
    (*[a], [b], [c] is 3 lists, [a; b; c] is a list*)

    let stringLength_2  (lst: string list) =
      lst |> List.map (fun s -> s.Length)
    
    let rec keepEven (lst: int list) =
      match lst with
      | [] -> []
      | head :: tail ->
          if (head % 2 = 0) then head :: keepEven tail
          else keepEven tail
    //keepEven [1; 2; 3; 4];;

    let keepEven_2 (lst: int list) = 
      let even x = x % 2 = 0
      lst |> List.filter even
    
    let rec keepLengthGT5 (lst: string list) = 
      match lst with
      | [] -> []
      | head :: tail ->
          if head.Length > 5 then head :: keepLengthGT5 tail
          else keepLengthGT5 tail
    //keepLengthGT5_2 ["hi"; "hello"; "Heeeyy"];;
        
    let keepLengthGT5_2 (lst: string list) =
      lst |> List.filter (fun s -> s.Length > 5)
    
    //sumPositive [1; -2; 3; 0; -1];;
    let sumPositive (lst: int list) : int =
      let rec loop acc input =
        match input with
        | [] -> acc
        | head :: tail -> 
            if head > 0 then loop (acc + head) tail
            else loop acc
      loop 0 lst
        
    let rec sumPositive_2 (lst: int list) = 
      List.fold (fun acc x ->
        if x > 0 then acc + x else acc) 0 lst
    //sumPositive_3 [1; -2; 3; 0; -1];;
    
    (* "," sepererer elementer i tupler, 
       ";" sepererer elementer i en liste !!!*)
    
    let rec sumPositive_3 (lst: int list) = 
      lst |> List.filter (fun n -> n > 0) |> List.fold (fun acc x -> acc + x) 0
    (* List.fold bruger 3 args: accumulator, status, startværdi *)

    (* Alternatively:
      
      let sumPositive_3 lst =
          lst
          |> List.filter (fun n -> n > 0)
          |> List.fold (+) 0
    
    *)

  //YELLOW
    let add5mul3_3 (f: 'a -> int) (x: 'a) = f x |> add5_2 |> mul3_2
 
    let rec mergeFuns (fs: ('a -> 'a) list ) = List.fold (fun acc f -> acc >> f) id fs
    (*Identity function id: fun x -> x*)

    let removeOddIdx xs = List.fold ()

  //RED
    let rec facFuns _ = failwith "not implemented"
        
    let fac_2 _ = failwith "not implemented"

    let weird _ = failwith "not implemented"
    
    let insert _= failwith "not implemented"
                
    let rec permutations _ = failwith "not implemented"