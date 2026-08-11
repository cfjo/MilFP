let double x = x * 2
//becomes ->
let doubleC x k = k (x * 2)
//where k is the function, and x is the value that will be doubled,
//and then put into the function
//for example
doubleC 5 (fun x -> x + 10)
//the result is 20. The continuation receives 10, then adds 10

let addC x y k = k (x + y)
//add 2 numbers, then apply the function to the result

let calculateC x k =
    let doubled = x * 2
    let added = doubled + 5
    k added

    (*alternatively:
        let calculateC x k =
            doubleC x (fun doubled ->
                addC doubled 5 k)
    *)

let f x k =
    k (x + 2)

f 5 (fun x -> x * 10)
//this returns 70