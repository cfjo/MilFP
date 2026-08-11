let delayedMessage () =
    async {
        do! Async.Sleep 2000
        return "Hello after 2 seconds!"
    }

let result = delayedMessage ()