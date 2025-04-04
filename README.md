
# LexYacc Parser
LexYacc parser that generates parser code in C#

![enter image description here](https://r96922081.github.io/LexYacc/arch.png)

## Create a Calculator

[cal.l](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccInput/cal.l) (input)\
[cal.y](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccInput/cal.y) (input) \
[cal.cs](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccOutput/cal.cs)  (generated)\
\
Use generated cal.cs 

    cal.Parse("2 * 3 + 6 / 3"); // returns 8
    
## Create a C code syntax tree parser

[c_grammar.l](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccInput/c_grammar.l) (input)\
[c_grammar.y](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccInput/c_grammar.y) (input)\
[c_grammar.cs](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccOutput/c_grammar.cs) (generated)\
\
Use generated c_grammar.cs:

    string input = @"
    void main(int a, int b) 
    {
        int c = 2 + 3;
        return c * a - 5;
    }";
    c_grammar.Parse(input);

![enter image description here](https://r96922081.github.io/LexYacc/syntax_tree.png)

## Usage:

Feed cal.l & cal.y, it will generate parser cal.cs

    LexYaccCodeGen.GenCode("D:/cal.l", "D:/cal.y", "D:/", "cal");

Call cal.Parse()

    cal.Parse("2 * 3 + 6 / 3"); // returns 8

## Dependeny
I use my own [regular expression engine](https://github.com/r96922081/Regex) in Lexer

## Real Use Case of My Project
[C Compiler](https://github.com/r96922081/C-Compiler)\
[Relational Database](https://github.com/r96922081/Relational-Database)
