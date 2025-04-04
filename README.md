# LexYacc Parser
LexYacc parser that generates parser code in C#

![enter image description here](https://r96922081.github.io/LexYacc/arch.png)

## Create a Calculator

[cal.l](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccInput/cal.l)\
[cal.y](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccInput/cal.y)
    
    cal.Parse("2 * 3 + 6 / 3"); // return 8
    
## Create a C code syntax tree parser

[c_grammar.l](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccInput/c_grammar.l)\
[c_grammar.y](https://github.com/r96922081/LexYacc/blob/main/Ut/LexYaccInput/c_grammar.y)

input.txt:

    int main(int a) 
    {
        int b = 3;
        return a * b - 4;
    }
　 

    c_grammar.Parse("input.txt");

![enter image description here](https://r96922081.github.io/LexYacc/syntax_tree.png)

## How to Use

Feed cal.l & cal.y, it will create cal.cs at D:

    LexYaccCodeGen.GenCode("D:/cal.l", "D:/cal.y", "D:/", "cal");

include cal.cs into your project and call cal.Parse()

    cal.Parse("2 * 3 + 6 / 3"); // output 8

## Dependeny
I use my own [regular expression engine](https://github.com/r96922081/Regex) in Lexer

## Real Use Case of My Project
[C Compiler](https://github.com/r96922081/C-Compiler)\
[Relational Database](https://github.com/r96922081/Relational-Database)
