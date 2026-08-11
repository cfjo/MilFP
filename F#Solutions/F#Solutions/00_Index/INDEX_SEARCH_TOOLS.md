# F# Solution Index Quick Start

## Recommended exam workflow

1. Open the `F#Solutions` folder in VS Code.
2. Open `00_Index/SOLUTION_INDEX.md`.
3. Search the index first with `Ctrl+F`.
4. When you find a hit like `ExamSolutions/Exam2025N.fs:268`, open that file and press `Ctrl+G`, then enter `268`.
5. If the index is not enough, use global search with `Ctrl+Shift+F`.

## VS Code global search settings

Files to include:

```text
**/*.{fs,fsi,fsx}
```

Files to exclude:

```text
**/bin/**, **/obj/**, **/.vs/**, **/.git/**, **/.idea/**, **/.vscode/**, **/.dotnet/**, **/.nuget/**, **/packages/**
```

## Useful index searches

Search these inside `SOLUTION_INDEX.md`:

```text
parser-combinators
```

Finds parser-related examples such as `FParsecLight`, `JParsec`, `pstring`, `many`, `choice`, `.>>`, `>>.`, and `<|>`.

```text
tail-recursion/accumulator
```

Finds examples involving recursive helpers, accumulators, `aux`, `loop`, `go`, and folds.

```text
agents/mailbox
```

Finds examples involving `MailboxProcessor`, `Post`, `PostAndReply`, `Receive`, and agent-style concurrency.

```text
state/interpreter
```

Finds interpreter/state-related examples such as memory, environment, lookup, assignment, and state-monad-style code.

```text
option
```

Finds examples involving `Some`, `None`, and `Option`.

```text
result/error
```

Finds examples involving `Ok`, `Error`, and `Result`.

## Useful raw regex searches

Find function definitions:

```regex
^\s*let\s+(rec\s+)?\w+
```

Find recursive function definitions:

```regex
^\s*let\s+rec\s+\w+
```

Find type definitions:

```regex
^\s*type\s+\w+
```

Find pattern match branches:

```regex
^\s*\|\s*.*->
```

Find option examples:

```regex
\b(Some|None|Option\.map|Option\.bind)\b
```

Find result examples:

```regex
\b(Ok|Error|Result\.map|Result\.bind)\b
```

Find parser combinator examples:

```regex
\b(pstring|pchar|many|many1|choice|attempt|satisfy)\b|\.>>|>>\.|<\|>
```
