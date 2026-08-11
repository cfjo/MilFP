#!/usr/bin/env python3
"""
F# solution indexer for exam-aid folders.

Usage:
    python fsharp_solution_indexer.py "C:\\path\\to\\F#Solutions"
    python fsharp_solution_indexer.py . --out SOLUTION_INDEX.md

What it does:
- Scans .fs, .fsi, and .fsx files.
- Ignores build/tooling junk: bin, obj, .vs, .git, .idea, .vscode, .dotnet, .nuget.
- Extracts question headings, modules, let/let rec definitions, type definitions, and rough topic tags.
- Writes one Markdown index you can search in VS Code.
"""

from __future__ import annotations

import argparse
import re
from collections import Counter, defaultdict
from dataclasses import dataclass, field
from pathlib import Path
from typing import Iterable

SOURCE_EXTS = {".fs", ".fsi", ".fsx"}
IGNORE_DIRS = {".vs", "bin", "obj", ".git", ".idea", ".vscode", ".dotnet", ".nuget", "packages"}

TOPIC_PATTERNS: dict[str, list[re.Pattern[str]]] = {
    # Core recursion and evaluation patterns
    "recursion": [
        re.compile(r"\blet\s+rec\b"),
        re.compile(r"\brec(?:ursive|ursion)?\b", re.I),
    ],
    "tail-recursion/accumulator": [
        re.compile(r"\b(acc|accumulator|accu|aux|loop|go)\b", re.I),
        re.compile(r"\blet\s+rec\s+(?:aux|loop|go|iter|tr)\b", re.I),
        re.compile(r"\bList\.fold\b|\bArray\.fold\b|\bSeq\.fold\b"),
    ],
    "continuations/CPS": [
        re.compile(r"\b(cont|continuation|cps)\b", re.I),
        re.compile(r"\blet\s+rec\s+\w+\s+.*\bk\b", re.I),
        re.compile(r"\bfun\s+\w+\s*->\s*\w+\s+\w+", re.I),
    ],

    # Pattern matching and algebraic modelling
    "pattern-matching": [
        re.compile(r"\bmatch\b"),
        re.compile(r"\bfunction\b"),
        re.compile(r"^\s*\|\s*.*->", re.M),
        re.compile(r"\bwhen\b"),
    ],
    "types/DU/records": [
        re.compile(r"^\s*type\s+\w+", re.M),
        re.compile(r"^\s*\|\s*[A-Z]\w*", re.M),
        re.compile(r"\{\s*[A-Za-z_][\w']*\s*:", re.M),
        re.compile(r"\bwith\s+member\b|\bmember\s+\w+", re.I),
    ],
    "records/tuples": [
        re.compile(r"\{\s*[A-Za-z_][\w']*\s*=", re.M),
        re.compile(r"\{\s*[A-Za-z_][\w']*\s*:", re.M),
        re.compile(r"\([A-Za-z_][\w']*\s*,\s*[A-Za-z_0-9]", re.M),
        re.compile(r"\bfst\b|\bsnd\b"),
    ],
    "domain-modeling/invariants": [
        re.compile(r"\b(invariant|valid|validate|normalize|well-formed|wellformed)\b", re.I),
        re.compile(r"\bprivate\b|\bcreate\b|\bmake\b|\btryCreate\b", re.I),
        re.compile(r"\binvalidArg\b|\bfailwith\b|\braise\b"),
    ],

    # Collections and higher-order style
    "lists": [
        re.compile(r"\bList\."),
        re.compile(r"::"),
        re.compile(r"\[\]"),
        re.compile(r"\[[^\]\n;]*(?:;|\.{2})[^\]\n]*\]"),
    ],
    "maps/sets": [
        re.compile(r"\bMap\.|\bSet\."),
        re.compile(r"\bMap<|\bSet<"),
        re.compile(r"\bMap\.empty\b|\bSet\.empty\b"),
        re.compile(r"\badd\b|\bremove\b|\bcontains\b|\bfind\b|\btryFind\b", re.I),
    ],
    "arrays/matrices": [
        re.compile(r"\bArray2D\b|\bArray\."),
        re.compile(r"\barray\b|\bmatrix\b|\bmatrices\b", re.I),
        re.compile(r"\.\[\s*[^;\]]+\s*\]"),
        re.compile(r"\.\[\s*[^;\]]+\s*,\s*[^;\]]+\s*\]"),
    ],
    "higher-order-functions": [
        re.compile(r"\b(map|filter|fold|foldBack|reduce|collect|choose|exists|forall|iter|sumBy|sortBy|groupBy|partition)\b"),
        re.compile(r"\bfun\s+[^\n]*->"),
        re.compile(r"\(\s*>>\s*\)|\(\s*<<\s*\)|\s>>\s|\s<<\s"),
        re.compile(r"\|>|<\|"),
    ],
    "folds/traversals": [
        re.compile(r"\b(List|Array|Seq)\.(fold|foldBack|reduce|scan)\b"),
        re.compile(r"\bfold\b|\bfoldBack\b|\btraverse\b|\bvisit\b", re.I),
    ],
    "sequences/lazy": [
        re.compile(r"\bseq\s*\{|\byield\b|\byield!\b"),
        re.compile(r"\bSeq\."),
        re.compile(r"\bunfold\b|\btake\b|\bskip\b|\bdelay\b|\blazy\b|Lazy<", re.I),
        re.compile(r"\bIEnumerable\b|\bIEnumerator\b"),
    ],

    # Options, results, errors, and partiality
    "option": [
        re.compile(r"\b(Some|None|Option\.)\b"),
        re.compile(r"\btry[A-Z]\w*"),
    ],
    "result/error": [
        re.compile(r"\b(Ok|Error|Result\.)\b"),
        re.compile(r"\bChoice\d+Of\d+\b"),
    ],
    "exceptions/partial-functions": [
        re.compile(r"\b(exception|try\s+.*with|with\s+ex|raise|failwith|invalidArg|invalidOp)\b", re.I | re.S),
        re.compile(r"\b(head|tail|find|item|get)\b", re.I),
    ],

    # Trees, expressions, and symbolic structures
    "trees": [
        re.compile(r"\b(tree|leaf|node|branch|subtree|children)\b", re.I),
        re.compile(r"^\s*\|\s*(Leaf|Node|Branch|Tip|Empty)\b", re.M),
    ],
    "binary-search-trees": [
        re.compile(r"\b(BST|binary\s+search\s+tree|search\s+tree)\b", re.I),
        re.compile(r"\b(insert|delete|contains|member|lookup)\b.*\b(tree|bst)\b", re.I | re.S),
    ],
    "expressions/AST/interpreter": [
        re.compile(r"\b(expr|expression|AST|eval|interpret|interpreter)\b", re.I),
        re.compile(r"^\s*\|\s*(Num|Const|Var|Add|Sub|Mul|Div|Let|If|While|Assign|Lambda|App)\b", re.M),
        re.compile(r"\b(declare|lookup|assign|environment|env|memory|store)\b", re.I),
    ],

    # Parser combinators and grammars
    "parser-combinators": [
        re.compile(r"\b(pstring|pchar|many|many1|choice|attempt|satisfy|between|sepBy|sepBy1|chainl1|spaces|eof)\b"),
        re.compile(r"\b(Parser|JParsec|FParsecLight|Parsec|runParser)\b"),
        re.compile(r"\.>>|>>\.|\.>>\.|<\|>"),
    ],
    "grammars/lexing": [
        re.compile(r"\b(grammar|terminal|nonterminal|token|lexer|lex|parse|parser)\b", re.I),
        re.compile(r"\bRegex\b|System\.Text\.RegularExpressions"),
    ],

    # Computation expressions and monadic style
    "computation-expressions/monads": [
        re.compile(r"\bBind\b|\bReturn\b|\bReturnFrom\b|\bZero\b|\bDelay\b|\bRun\b|\bCombine\b|\bFor\b|\bYield\b|\bYieldFrom\b"),
        re.compile(r"\blet!\b|\bdo!\b|\breturn!\b|\breturn\b"),
        re.compile(r"\b(StateMonad|OptionBuilder|ResultBuilder|MaybeBuilder|builder|monad)\b", re.I),
    ],
    "state/interpreter": [
        re.compile(r"\b(StateMonad|StateBuilder|state\s*\{|getState|putState|modifyState)\b", re.I),
        re.compile(r"\b(Language|Memory|State|Store|Env|environment|lookup|assign|declare|push|pop)\b"),
    ],

    # Imperative, mutable, async, parallel, and actor-like code
    "mutable-state/imperative": [
        re.compile(r"\bmutable\b|<-\s*|:=|!\w+"),
        re.compile(r"\bref\b|\bwhile\b|\bfor\b"),
        re.compile(r"\bResizeArray\b|System\.Collections\.Generic"),
    ],
    "async/parallel": [
        re.compile(r"\basync\s*\{|\blet!\b|\bdo!\b"),
        re.compile(r"\bAsync\."),
        re.compile(r"\bParallel\b|Task<|Task\.|Thread\b", re.I),
    ],
    "agents/mailbox": [
        re.compile(r"\bMailboxProcessor\b|\bAgent\b"),
        re.compile(r"\bPostAndReply\b|\bPost\b|\bReceive\b|\bReply\b"),
        re.compile(r"\binbox\b|\bmessage\b|\bmsg\b", re.I),
    ],

    # Text, files, IO, and Kattis-style competitive programming
    "kattis/IO": [
        re.compile(r"\bConsole\.ReadLine\b|\bstdin\b|\bprintfn\b|\bprintf\b|\bReadLine\b"),
        re.compile(r"\bsplit\b|\bSplit\b|\bTrim\b|\bint\b|\bfloat\b"),
    ],
    "strings/text/regex": [
        re.compile(r"\bstring\b|\bString\b|\bchar\b|\bChar\b"),
        re.compile(r"\bSubstring\b|\bContains\b|\bStartsWith\b|\bEndsWith\b|\bReplace\b|\bSplit\b|\bTrim\b"),
        re.compile(r"\bRegex\b|System\.Text\.RegularExpressions"),
    ],
    "files/resources": [
        re.compile(r"\bFile\.|\bDirectory\.|\bStreamReader\b|\bStreamWriter\b"),
        re.compile(r"\buse\s+\w+|\bIDisposable\b"),
    ],

    # Algorithms and maths
    "numeric/math": [
        re.compile(r"\b(gcd|factorial|fib|prime|sqrt|pow|abs|min|max|sum|product)\b", re.I),
        re.compile(r"\bBigInteger\b|\bMath\."),
    ],
    "algorithms/sorting/scheduling": [
        re.compile(r"\b(sort|sortBy|sortWith|compare|comparison|binary\s+search|search)\b", re.I),
        re.compile(r"\b(interval|schedule|scheduling|greedy|priority|queue|stack)\b", re.I),
        re.compile(r"\bGCD\b|\bfactorial\b|\bbinary\b", re.I),
    ],
    "graphs/search": [
        re.compile(r"\b(graph|vertex|edge|node|neighbour|neighbor|adjacent|path|cycle)\b", re.I),
        re.compile(r"\bBFS\b|\bDFS\b|\bDijkstra\b|\bshortest\b", re.I),
    ],

    # Exam-question clues in comments/scaffolds
    "type-inference/code-comprehension": [
        re.compile(r"\b(what\s+is\s+the\s+type|give\s+the\s+type|infer\s+the\s+type|type\s+of)\b", re.I),
        re.compile(r"\b(explain|describe|what\s+does|evaluate|reduce|step\s+by\s+step)\b", re.I),
        re.compile(r"\b(name\s+the\s+function|appropriate\s+name|is\s+it\s+tail\s+recursive)\b", re.I),
    ],
    "tests/examples": [
        re.compile(r"\b(assert|Expect|NUnit|Xunit|FsCheck|test|property)\b", re.I),
        re.compile(r"\bexample\b|\bsample\b", re.I),
    ],
}

QUESTION_RE = re.compile(
    r"""
    ^\s*(?P<comment>//|\(\*)\s*
    (?P<label>
        (?:Question\s+\d+(?:\.\d+)?(?:\s*[:\-].*)?)|
        (?:\d+\s*[:.]\s*[^*)]+)
    )
    """,
    re.I | re.X,
)

DEF_RES = [
    ("module", re.compile(r"^\s*module\s+([A-Za-z_][\w'.]*)")),
    ("type", re.compile(r"^\s*type\s+([A-Za-z_][\w']*)")),
    ("let rec", re.compile(r"^\s*let\s+rec\s+(?:private\s+)?([A-Za-z_][\w']*)")),
    ("let", re.compile(r"^\s*let\s+(?:private\s+)?([A-Za-z_][\w']*)")),
]

@dataclass
class Defn:
    kind: str
    name: str
    line: int

@dataclass
class Section:
    title: str
    line: int
    tags: list[str]
    defns: list[Defn] = field(default_factory=list)

@dataclass
class FileInfo:
    rel_path: str
    source_group: str
    lines: int
    tags: list[str]
    defns: list[Defn]
    sections: list[Section]


def is_source(path: Path) -> bool:
    return path.is_file() and path.suffix.lower() in SOURCE_EXTS and not any(part in IGNORE_DIRS for part in path.parts)


def classify(text: str) -> list[str]:
    tags = []
    for topic, pats in TOPIC_PATTERNS.items():
        if any(p.search(text) for p in pats):
            tags.append(topic)
    return tags


def extract_defns(lines: list[str]) -> list[Defn]:
    out: list[Defn] = []
    for n, line in enumerate(lines, 1):
        for kind, rx in DEF_RES:
            m = rx.match(line)
            if m:
                # Avoid counting "let rec" as plain "let" too.
                if kind == "let" and re.match(r"^\s*let\s+rec\b", line):
                    continue
                out.append(Defn(kind, m.group(1), n))
                break
    return out


def section_for_line(sections: list[tuple[int, str]], line_no: int) -> str:
    current = "whole file"
    for ln, title in sections:
        if ln <= line_no:
            current = title
        else:
            break
    return current


def extract_sections(lines: list[str], defns: list[Defn]) -> list[Section]:
    headings: list[tuple[int, str]] = []
    for n, line in enumerate(lines, 1):
        m = QUESTION_RE.match(line)
        if m:
            title = re.sub(r"\s+", " ", m.group("label").strip())
            # Remove trailing comment end if present.
            title = title.replace("*)", "").strip()
            headings.append((n, title))

    if not headings:
        return []

    sections: list[Section] = []
    for idx, (start_line, title) in enumerate(headings):
        end_line = headings[idx + 1][0] - 1 if idx + 1 < len(headings) else len(lines)
        text = "\n".join(lines[start_line - 1:end_line])
        sec_defns = [d for d in defns if start_line <= d.line <= end_line]
        sections.append(Section(title=title, line=start_line, tags=classify(text), defns=sec_defns[:20]))
    return sections


def scan(root: Path) -> list[FileInfo]:
    files = sorted(p for p in root.rglob("*") if is_source(p))
    infos: list[FileInfo] = []
    for p in files:
        text = p.read_text(encoding="utf-8", errors="replace")
        lines = text.splitlines()
        rel = p.relative_to(root).as_posix()
        source_group = rel.split("/", 1)[0]
        defns = extract_defns(lines)
        infos.append(
            FileInfo(
                rel_path=rel,
                source_group=source_group,
                lines=len(lines),
                tags=classify(text),
                defns=defns,
                sections=extract_sections(lines, defns),
            )
        )
    return infos


def fmt_defns(defns: Iterable[Defn], limit: int = 12) -> str:
    parts = [f"`{d.kind} {d.name}`@{d.line}" for d in list(defns)[:limit]]
    return ", ".join(parts) if parts else "-"


def write_index(root: Path, infos: list[FileInfo], out_path: Path) -> None:
    total_lines = sum(i.lines for i in infos)
    group_counts = Counter(i.source_group for i in infos)
    topic_to_locations: dict[str, list[tuple[str, int, str]]] = defaultdict(list)

    for info in infos:
        # Prefer question-level sections where available; otherwise file-level.
        if info.sections:
            for sec in info.sections:
                for tag in sec.tags:
                    topic_to_locations[tag].append((info.rel_path, sec.line, sec.title))
        else:
            for tag in info.tags:
                topic_to_locations[tag].append((info.rel_path, 1, "whole file"))

    md: list[str] = []
    md.append("# F# Solution Index\n")
    md.append("Generated by `fsharp_solution_indexer.py`. Search this file first, then jump to the real source file/line in VS Code.\n")

    md.append("## How to use this index\n")
    md.append("1. Open your `F#Solutions` folder in VS Code.\n")
    md.append("2. Open this `SOLUTION_INDEX.md`.\n")
    md.append("3. Search this index with `Ctrl+F`, for example `parser-combinators`, `tail-recursion`, `MailboxProcessor`, `Question 3.2`, `StateMonad`, or `Array2D`.\n")
    md.append("4. Copy the path/line reference, e.g. `ExamSolutions/Exam2025N.fs:275`, then open that file and use `Ctrl+G` to jump to the line.\n")
    md.append("5. For raw code search across all source files, use `Ctrl+Shift+F` with `files to include: **/*.{fs,fsi,fsx}` and exclude `**/bin/**, **/obj/**, **/.vs/**`.\n")

    md.append("## Folder strategy\n")
    md.append("Keep the three source folders separate by origin, but do **not** duplicate files into topic folders. Use this index as the many-to-many topic map. One exam answer can involve recursion, pattern matching, options, and parser combinators at the same time.\n")
    md.append("\nRecommended layout:\n")
    md.append("```text\nF#Solutions/\n├── 00_Index/\n│   ├── SOLUTION_INDEX.md\n│   └── fsharp_solution_indexer.py\n├── ExamSolutions/\n├── Assignments/\n└── Kattis/\n```\n")

    md.append("## Scan summary\n")
    md.append(f"- Source files indexed: **{len(infos)}**\n")
    md.append(f"- Total source lines indexed: **{total_lines}**\n")
    for group, count in sorted(group_counts.items()):
        md.append(f"- `{group}`: **{count}** source files\n")
    md.append("- Ignored build/tooling folders: `.vs`, `bin`, `obj`, `.git`, `.idea`, `.vscode`, `.dotnet`, `.nuget`, `packages`\n")

    md.append("## Topic index\n")
    for topic in sorted(topic_to_locations):
        locs = topic_to_locations[topic]
        md.append(f"\n### {topic} ({len(locs)} locations)\n")
        for rel, line, title in locs[:60]:
            md.append(f"- `{rel}:{line}` — {title}\n")
        if len(locs) > 60:
            md.append(f"- ... {len(locs) - 60} more locations\n")

    md.append("\n## Detailed source index\n")
    for group in ["ExamSolutions", "Assignments", "Kattis"]:
        md.append(f"\n# {group}\n")
        for info in [i for i in infos if i.source_group == group]:
            md.append(f"\n## `{info.rel_path}`\n")
            md.append(f"- Lines: {info.lines}\n")
            md.append(f"- File tags: {', '.join(info.tags) if info.tags else '-'}\n")
            md.append(f"- Definitions: {fmt_defns(info.defns)}\n")
            if info.sections:
                md.append("\nSections:\n")
                for sec in info.sections:
                    md.append(f"- `{info.rel_path}:{sec.line}` — **{sec.title}**\n")
                    md.append(f"  - Tags: {', '.join(sec.tags) if sec.tags else '-'}\n")
                    md.append(f"  - Definitions: {fmt_defns(sec.defns, limit=10)}\n")

    out_path.write_text("".join(md), encoding="utf-8")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("root", nargs="?", default=".", help="Root folder to scan, usually F#Solutions")
    parser.add_argument("--out", default="SOLUTION_INDEX.md", help="Output Markdown file")
    args = parser.parse_args()

    root = Path(args.root).resolve()
    if not root.exists():
        raise SystemExit(f"Root does not exist: {root}")

    infos = scan(root)
    out_path = Path(args.out)
    if not out_path.is_absolute():
        out_path = root / out_path
    write_index(root, infos, out_path)
    print(f"Indexed {len(infos)} source files -> {out_path}")


if __name__ == "__main__":
    main()
