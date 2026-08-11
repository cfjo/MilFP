open JParsec.TextParser
//parse one digit
let digit =
    satisfy 
        (fun c -> c >= '0' && c <= '9')
        |>> (fun c -> int c - int '0')