module Assignment3
    
    let add5 x = x + 5
        
    let mul3 x = x * 3

    let add5mul3 x = x |> add5 |> mul3

    let add5mul3_2 = add5 >> mul3
    
    let add5_2 (f: 'a -> int) (x: 'a) = (f >> add5) x

    let mul3_2 _ = failwith "not implemented"
   
    let rec downto4 _ = failwith "not implemented"
                
    let fac _ = failwith "not implemented"
    
    let range _ = failwith "not implemented"
    
    let rec double _ = failwith "not implemented"
        
    let double_2 _ = failwith "not implemented"
    
    let rec stringLength _ = failwith "not implemented"
        
    let stringLength_2 _ = failwith "not implemented"
    
    let rec keepEven _ = failwith "not implemented"
    
    let keepEven_2 _ = failwith "not implemented"
    
    let rec keepLengthGT5 _ = failwith "not implemented"
        
    let keepLengthGT5_2 _ = failwith "not implemented"
    
    
    let rec sumPositive _ = failwith "not implemented"
        
    let rec sumPositive_2 _ = failwith "not implemented"
        
    let rec sumPositive_3 _ = failwith "not implemented"
        
  //YELLOW
    let add5mul3_3 _ = failwith "not implemented"
    
 
    let rec mergeFuns _ = failwith "not implemented"
        
    let rec facFuns _ = failwith "not implemented"
        
    let fac_2 _ = failwith "not implemented"

    let removeOddIdx _= failwith "not implemented"
        
    
    let weird _ = failwith "not implemented"
    
   
    let insert _= failwith "not implemented"
                
    let rec permutations _ = failwith "not implemented"