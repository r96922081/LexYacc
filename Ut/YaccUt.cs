using LexYaccNs;

using System.Diagnostics;

public class YaccUt
{
    public static List<Terminal> tokens = new List<Terminal>();

    public static void Check(bool b)
    {
        if (!b)
            Trace.Assert(false);
    }

    public static void UtFeed1()
    {
        string line1 = "a: 'A' 'B'";
        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == true);


        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == false);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == false);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == false);
    }


    public static void UtFeed2()
    {
        string line1 = "a: 'A' 'B' | 'A' 'C' ";
        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        Check(yacc.Feed(tokens) == false);
    }


    public static void UtFeed3()
    {
        string line1 = "a: 'A' b;";
        string line2 = "b: 'B'";

        Yacc yacc = new Yacc(line1 + line2);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == true);
    }


    public static void UtFeed4()
    {
        string line1 = "a: 'A' b;";
        string line2 = "b: c;";
        string line3 = "c: 'C'";

        Yacc yacc = new Yacc(line1 + line2 + line3);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == true);
    }


    public static void UtFeed5()
    {
        string line1 = "a: 'A' | 'B'";

        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == true);
    }


    public static void UtFeed6()
    {
        string line1 = "a: b;";
        string line2 = "b: 'A' | 'B'";

        Yacc yacc = new Yacc(line1 + line2);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == false);
    }


    public static void UtFeed7()
    {
        string line1 = "a: 'A' b 'E';";
        string line2 = "b: 'B' c | 'B' d;";
        string line3 = "c: 'C';";
        string line4 = "d: 'D' 'F'";


        Yacc yacc = new Yacc(line1 + line2 + line3 + line4);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == false);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('E'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        tokens.Add(Terminal.BuildConstCharTerminal('F'));
        tokens.Add(Terminal.BuildConstCharTerminal('E'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('G'));
        Check(yacc.Feed(tokens) == false);
    }

    public static void UtFeed8()
    {
        string line1 = "a: 'A' a | 'A'";

        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);


        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == false);
    }

    public static void UtFeed9()
    {
        string line1 = "a: 'A' a 'B' | 'C' ";

        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == false);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == false);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == true);
    }



    public static void UtFeed10()
    {
        string line1 = "a: a 'A' | 'A' ";

        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == false);
    }


    public static void UtFeed11()
    {
        string line1 = "a: a 'A' | 'B' ";

        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B')); ;
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == false);
    }

    public static void UtFeed12()
    {
        string line1 = "a: 'A' b 'C';";
        string line2 = "b: | 'B'";

        Yacc yacc = new Yacc(line1 + line2);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == true);
    }

    public static void UtFeed13()
    {
        string line1 = "a: a 'A' | a 'B' | 'C' | 'D'";

        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == false);
    }

    public static void UtFeed14()
    {
        string line1 = "a: b 'A';";
        string line2 = "b:  | 'B'";

        Yacc yacc = new Yacc(line1 + line2);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == false);
    }

    public static void UtFeed15()
    {
        string line1 = "a: 'A' b 'C'; ";
        string line2 = "b:  | 'B'";

        Yacc yacc = new Yacc(line1 + line2);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == false);
    }

    public static void UtFeed16()
    {
        string line1 = "a: 'A' b c; ";
        string line2 = "b:  | 'B';";
        string line3 = "c:  | 'C';";
        string line4 = "d:  | 'D';";

        Yacc yacc = new Yacc(line1 + line2 + line3 + line4);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);
    }

    public static void UtFeed17()
    {
        string s = @"
%{
%}

%token <strValue> VOID

%%
a: VOID
%%
";

        Yacc yacc = new Yacc(s);
        tokens.Add(Terminal.BuildToken("VOID"));
    }

    public static void UtFeed18()
    {
        string s = @"
%{
%}

%token <strValue> VOID INT

%%
a: VOID INT 'a'
%%
";

        Yacc yacc = new Yacc(s);
        tokens.Add(Terminal.BuildToken("VOID"));
        tokens.Add(Terminal.BuildToken("INT"));
        tokens.Add(Terminal.BuildConstCharTerminal('a'));
    }

    public static void UtFeed19()
    {
        string s = @"
%{
%}

%token <int> NUMBER
%type <int> exp term

%%
a: 'A' b 'C' ;
b: 'D' c 'F';
c: 'G' d 'H';
d: 'I' 'J';
  
term: NUMBER;
%%
";

        Yacc yacc = new Yacc(s);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        tokens.Add(Terminal.BuildConstCharTerminal('G'));
        tokens.Add(Terminal.BuildConstCharTerminal('I'));
        tokens.Add(Terminal.BuildConstCharTerminal('J'));
        tokens.Add(Terminal.BuildConstCharTerminal('H'));
        tokens.Add(Terminal.BuildConstCharTerminal('F'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));

        Check(yacc.Feed(tokens) == true);
    }

    public static void UtFeed20()
    {
        string s = @"
%{
%}

%token <int> NUMBER
%type <int> exp term

%%
exp:
  exp '+' term {$$ = $1 + $3;}
  | term {$$ = $1;}
  ;
  
term:
  term '*' NUMBER { $$ = $1 * $3;}
  | NUMBER {$$ = $1;}
  ;
%%
";

        Yacc yacc = new Yacc(s);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER"));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('+'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('*'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        Check(yacc.Feed(tokens) == true);
    }

    public static void UtFeed21()
    {
        string s = @"
%{
%}

%token <int> NUMBER
%type <int> exp term

%%
cal:
 exp {printf(""Result = %d\n"", $1); }
 ;
 
exp:
  exp '+' term {$$ = $1 + $3;}
  | exp '-' term {$$ = $1 - $3;}
  | term {$$ = $1;}
  ;
  
term:
  term '*' NUMBER { $$ = $1 * $3;}
  | term '/' NUMBER { $$ = $1 / $3;}
  | NUMBER {$$ = $1;}
  ;
%%
";

        Yacc yacc = new Yacc(s);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('-'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        Check(yacc.Feed(tokens) == false);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('-'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('+'));
        Check(yacc.Feed(tokens) == false);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('-'));
        tokens.Add(Terminal.BuildConstCharTerminal('/'));
        Check(yacc.Feed(tokens) == false);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('-'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('*'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('+'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('/'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('/'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('-'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        tokens.Add(Terminal.BuildConstCharTerminal('-'));
        tokens.Add(Terminal.BuildToken("NUMBER"));
        Check(yacc.Feed(tokens) == true);
    }

    public static void UtFeed22()
    {
        string s = @"
%{
%}

%type <int> a

%%
a:
a '&' a
|
'A'
;
%%
";

        Yacc yacc = new Yacc(s);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('&'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('&'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('&'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);
    }

    public static void UtFeed23()
    {
        string line1 = "a: 'A' | 'A' 'B';";

        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        Check(yacc.Feed(tokens) == true);
    }

    public static void UtFeed24()
    {
        string line1 = @"
a:
b 'A'
;
b: 
'B' 
|
'B' b
;
";

        Yacc yacc = new Yacc(line1);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == true);
    }

    public static void UtFeed25()
    {
        string line1 = "a: b 'A'; ";
        string line2 = "b: c; ";
        string line3 = "c: d; ";
        string line4 = "d: a 'D'; ";

        /*
          a: c 'A';
          ->
          a: d 'A'
          ->
          d: d 'A' 'D'
         */

        Yacc yacc = new Yacc(line1 + line2 + line3 + line4);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('D'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        Check(yacc.Feed(tokens) == false);
    }

    public static void UtFeed26()
    {
        string line1 = "a: b 'A' | 'A'; ";
        string line2 = "b: c 'B'; ";
        string line3 = "c: a 'C'; ";

        /*
          a: c 'B' 'A' | 'A';
          ->
          c:  (c 'B' 'A' | 'A') x 'C'
          ->
          c: c 'B' 'A' 'C' | 'A' 'C'
         */

        Yacc yacc = new Yacc(line1 + line2 + line3);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        Check(yacc.Feed(tokens) == true);

        yacc.Rebuild();
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('X'));
        Check(yacc.Feed(tokens) == false);
    }

    private static void UtFeed()
    {
        UtFeed1();
        UtFeed2();
        UtFeed3();
        UtFeed4();
        UtFeed5();
        UtFeed6();
        UtFeed7();
        UtFeed8();
        UtFeed9();
        UtFeed10();
        UtFeed11();
        UtFeed12();
        UtFeed13();
        UtFeed14();
        UtFeed15();
        UtFeed16();
        UtFeed17();
        UtFeed18();
        UtFeed19();
        UtFeed20();
        UtFeed21();
        UtFeed22();
        UtFeed23();
        UtFeed24();
        //UtFeed25();
        //UtFeed26();
    }

    private static void UtBuild1()
    {
        string ruleSection = "a: 'A' 'B'";

        Yacc yacc = new Yacc(ruleSection);
        Check(yacc.productionRules.Count == 2);
        YaccRule pr = yacc.productionRules[1];
        Check(pr.lhs.name == "a");

        Check(pr.productions.Count == 1);

        Terminal symbol = (Terminal)pr.productions[0].symbols[0];
        Check(symbol.type == TerminalType.CONSTANT_CHAR);
        Check(symbol.constCharValue == "A");

        symbol = (Terminal)pr.productions[0].symbols[1];
        Check(symbol.type == TerminalType.CONSTANT_CHAR);
        Check(symbol.constCharValue == "B");
    }

    private static void UtBuild2()
    {
        string ruleSection = "a: 'A' 'B' | 'A' 'C' ;";

        Yacc yacc = new Yacc(ruleSection);
        Check(yacc.productionRules.Count == 2);
        YaccRule pr = yacc.productionRules[1];
        Check(pr.lhs.name == "a");

        Check(pr.productions.Count == 2);

        Terminal symbol = (Terminal)pr.productions[0].symbols[0];
        Check(symbol.type == TerminalType.CONSTANT_CHAR);
        Check(symbol.constCharValue == "A");

        symbol = (Terminal)pr.productions[0].symbols[1];
        Check(symbol.type == TerminalType.CONSTANT_CHAR);
        Check(symbol.constCharValue == "B");

        symbol = (Terminal)pr.productions[1].symbols[0];
        Check(symbol.type == TerminalType.CONSTANT_CHAR);
        Check(symbol.constCharValue == "A");

        symbol = (Terminal)pr.productions[1].symbols[1];
        Check(symbol.type == TerminalType.CONSTANT_CHAR);
        Check(symbol.constCharValue == "C");
    }

    private static void UtBuild3()
    {
        string ruleSection = @"
a: 'A';
b: 'B'

";

        Yacc yacc = new Yacc(ruleSection);
        Check(yacc.productionRules.Count == 3);

        YaccRule pr = yacc.productionRules[1];
        Check(pr.lhs.name == "a");
        Check(pr.productions.Count == 1);
        Terminal symbol = (Terminal)pr.productions[0].symbols[0];
        Check(symbol.type == TerminalType.CONSTANT_CHAR);
        Check(symbol.constCharValue == "A");

        pr = yacc.productionRules[2];
        Check(pr.lhs.name == "b");
        Check(pr.productions.Count == 1);
        symbol = (Terminal)pr.productions[0].symbols[0];
        Check(symbol.type == TerminalType.CONSTANT_CHAR);
        Check(symbol.constCharValue == "B");
    }

    private static void UtBuild4()
    {
        string line1 = "a: b;";
        string line2 = "b: 'B'";

        Yacc yacc = new Yacc(line1 + line2);
        Check(yacc.productionRules.Count == 3);

        YaccRule pr = yacc.productionRules[1];
        Check(pr.lhs.name == "a");
        Check(pr.productions.Count == 1);
        Nonterminal symbol = (Nonterminal)pr.productions[0].symbols[0];
        Check(symbol.name == "b");
    }

    private static void UtBuild5()
    {
        string line1 = "a: 'A' | | 'B';";
        string line2 = "b:  | 'C' | 'D'";

        Yacc yacc = new Yacc(line1 + line2);
        Check(yacc.productionRules.Count == 3);

        YaccRule pr = yacc.productionRules[1];
        Terminal t = (Terminal)pr.productions[0].symbols[0];
        Check(t.constCharValue == "A");
        t = (Terminal)pr.productions[1].symbols[0];
        Check(t.type == TerminalType.EMPTY);
        t = (Terminal)pr.productions[2].symbols[0];
        Check(t.constCharValue == "B");

        pr = yacc.productionRules[2];
        t = (Terminal)pr.productions[0].symbols[0];
        Check(t.type == TerminalType.EMPTY);
        t = (Terminal)pr.productions[1].symbols[0];
        Check(t.constCharValue == "C");
        t = (Terminal)pr.productions[2].symbols[0];
        Check(t.constCharValue == "D");
    }

    private static void UtBuild6()
    {
        string input = @"
a : 'A' ;
b : 'B' { bbb };
c : 'C';
d: a b | c;
e: 'e' |;
f: | 'f';
";
        List<YaccRule> rules = RuleSectionParser.Parse(input);
        Check(rules[0].lhs.name == "a");
        Check(((Terminal)rules[0].productions[0].symbols[0]).constCharValue == "A");
        Check(rules[0].productions[0].action == null);

        Check(rules[1].lhs.name == "b");
        Check(((Terminal)rules[1].productions[0].symbols[0]).constCharValue == "B");
        Check(rules[1].productions[0].action.Trim() == "bbb");

        Check(rules[2].lhs.name == "c");
        Check(((Terminal)rules[2].productions[0].symbols[0]).constCharValue == "C");

        Check(rules[3].lhs.name == "d");
        Check(((Nonterminal)rules[3].productions[0].symbols[0]).name == "a");
        Check(((Nonterminal)rules[3].productions[0].symbols[1]).name == "b");
        Check(((Nonterminal)rules[3].productions[1].symbols[0]).name == "c");

        Check(rules[4].lhs.name == "e");
        Check(((Terminal)rules[4].productions[0].symbols[0]).constCharValue == "e");
        Check(((Terminal)rules[4].productions[1].symbols[0]).type == TerminalType.EMPTY);

        Check(rules[5].lhs.name == "f");
        Check(((Terminal)rules[5].productions[0].symbols[0]).type == TerminalType.EMPTY);
        Check(((Terminal)rules[5].productions[1].symbols[0]).constCharValue == "f");
    }

    private static void UtBuild7()
    {
        string input = @"
a : 'A' '{' ';' '|' 
{
   abc // { | ;
} ;
b : 'B'
";
        List<YaccRule> rules = RuleSectionParser.Parse(input);
        Check(rules[0].lhs.name == "a");
        Check(((Terminal)rules[0].productions[0].symbols[0]).constCharValue == "A");
        Check(rules[0].productions[0].action != null);

        Check(rules[1].lhs.name == "b");
        Check(((Terminal)rules[1].productions[0].symbols[0]).constCharValue == "B");

    }

    /*
        a: a 'A' | 'B'  =>

        a: 'B' a2
        a2: 'A' a2 | empty
    */

    private static void UtBuild8()
    {
        List<YaccRule> prs = RuleSectionParser.Parse("a: a 'A' | 'B'");
        YaccRule pr1 = prs[0];
        YaccRule pr2 = prs[1];

        Check(pr1.lhs.name == "a");
        Production p1 = pr1.productions[0];
        Terminal t = (Terminal)p1.symbols[0];
        Check(t.constCharValue == "B");
        Nonterminal nt = (Nonterminal)p1.symbols[1];
        Check(nt.name == "a_LeftRecursionExpand");

        p1 = pr2.productions[0];
        Check(pr2.lhs.name == "a_LeftRecursionExpand");
        t = (Terminal)p1.symbols[0];
        Check(t.constCharValue == "A");
        nt = (Nonterminal)p1.symbols[1];
        Check(nt.name == "a_LeftRecursionExpand");

        Production p2 = pr2.productions[1];
        t = (Terminal)p2.symbols[0];
        Check(t.type == TerminalType.EMPTY);
    }

    /*
        a: a 'A' | a 'B' | 'C' | 'D' =>

        a: 'C' a2 | 'D' a2
        a2: 'A' a2 | 'B' a2 | empty
    */
    private static void UtBuild9()
    {
        List<YaccRule> prs = RuleSectionParser.Parse("a: a 'A' | a 'B' | 'C' | 'D'");
        YaccRule pr1 = prs[0];
        YaccRule pr2 = prs[1];

        Check(pr1.lhs.name == "a");

        Production p1 = pr1.productions[0];
        Terminal t = (Terminal)p1.symbols[0];
        Check(t.constCharValue == "C");
        Nonterminal nt = (Nonterminal)p1.symbols[1];
        Check(nt.name == "a_LeftRecursionExpand");

        Production p2 = pr1.productions[1];
        t = (Terminal)p2.symbols[0];
        Check(t.constCharValue == "D");
        nt = (Nonterminal)p2.symbols[1];
        Check(nt.name == "a_LeftRecursionExpand");

        Check(pr2.lhs.name == "a_LeftRecursionExpand");

        p1 = pr2.productions[0];
        t = (Terminal)p1.symbols[0];
        Check(t.constCharValue == "A");
        nt = (Nonterminal)p1.symbols[1];
        Check(nt.name == "a_LeftRecursionExpand");

        p2 = pr2.productions[1];
        Check(pr2.lhs.name == "a_LeftRecursionExpand");
        t = (Terminal)p2.symbols[0];
        Check(t.constCharValue == "B");
        nt = (Nonterminal)p2.symbols[1];
        Check(nt.name == "a_LeftRecursionExpand");

        Production p3 = pr2.productions[2];
        t = (Terminal)p3.symbols[0];
        Check(t.type == TerminalType.EMPTY);
    }

    private static void UtBuild()
    {
        UtBuild1();
        UtBuild2();
        UtBuild3();
        UtBuild4();
        UtBuild5();
        UtBuild6();
        UtBuild7();
        UtBuild8();
        UtBuild9();
    }

    private static void UtSecctionSplitter1()
    {
        string input = @"
%{
using abc;
%}

%token <strVal> a b
%type <astVal>  d

%%
a : 'A'
%%
";
        Section s = YaccRuleReader.SplitSecction(input, true);
        Check(s.definitionSection.Trim() == "using abc;");
        Check(s.typeSection.Trim() == "%token <strVal> a b\r\n%type <astVal>  d");
        Check(s.ruleSection.Trim() == "a : 'A'");
    }

    private static void UtSecctionSplitter2()
    {
        string input = @"
a : 'A'
";
        Section s = YaccRuleReader.SplitSecction(input, true);
        Check(s.definitionSection == null);
        Check(s.typeSection == null);
        Check(s.ruleSection.Trim() == "a : 'A'");
    }

    private static void UtSecctionSplitter3()
    {
        string input = @"
%{abc;// %{ %} %%
def;  %}

%token <strVal> a b
%type <astVal>  d

%% a : 'A' { // %{ %} %%
}  %%
";
        Section s = YaccRuleReader.SplitSecction(input, true);
        Check(s.definitionSection.Trim() == "abc;// %{ %} %%\r\ndef;");
        Check(s.typeSection.Trim() == "%token <strVal> a b\r\n%type <astVal>  d");
        Check(s.ruleSection.Trim() == "a : 'A' { // %{ %} %%\r\n}");
    }

    private static void UtSecctionSplitter()
    {
        UtSecctionSplitter1();
        UtSecctionSplitter2();
        UtSecctionSplitter3();
    }



    private static void UtAction1()
    {
        string s = @"
%{        
class TypeA 
{ 

}

class TypeB
{ 

}

class TypeC
{ 

}

class MyStr{
    public MyStr(string s){this.s = s;}
    public string s;
}
%}

%type <TypeA>  a
%type <TypeB>  b
%type <TypeC>  c
%token <MyStr> VOID

%%
a: 'A' c 'X' b VOID
{ 
    Console.WriteLine(""rule A lhs = "" + $$.ToString()); 
    Console.WriteLine($2.ToString()); 
    Console.WriteLine($4.ToString()); 
    Console.WriteLine($5.s);};
b: 'B' { Console.WriteLine(""it is b"");};
c: 'C' { Console.WriteLine(""it is c"");}
%%
";

        Yacc yacc = new Yacc(s);
        YaccCodeGen.GenCode(s, "UtYacc1", LexYaccUtil.GetGenFileFolder(), false);

#if !DisableGenCodeUt
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('X'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));

        tokens.Add(Terminal.BuildToken("VOID", new UtYacc1Ns.MyStr("void str 222")));
        bool ret = yacc.Feed(tokens);

        Console.WriteLine("\n\nUtAction1 output");
        yacc.route.startDFA.CallAction(UtYacc1Ns.YaccActions.CallAction);

#endif
    }

    private static void UtAction2()
    {
        string s = @"
%{        
%}
%type <string> a
%%
a: 'A' a {Console.WriteLine(""A with a"");} | 'A' {Console.WriteLine(""single A"");};
%%
";
        Yacc yacc = new Yacc(s);
        YaccCodeGen.GenCode(s, "UtYacc2", LexYaccUtil.GetGenFileFolder(), false);

#if !DisableGenCodeUt
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        yacc.Feed(tokens);

        Console.WriteLine("\n\nUtAction2 output");
        yacc.route.startDFA.CallAction(UtYacc2Ns.YaccActions.CallAction);
#endif
    }

    private static void UtAction3()
    {
        string s = @"
%{        
%}
%type <string> a
%%
a: a 'A' {Console.WriteLine(""A with a"");} | 'A' {Console.WriteLine(""single A"");};
%%
";
        Yacc yacc = new Yacc(s);
        YaccCodeGen.GenCode(s, "UtYacc3", LexYaccUtil.GetGenFileFolder(), false);

#if !DisableGenCodeUt
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        yacc.Feed(tokens);

        Console.WriteLine("\n\nUtAction3 output");
        yacc.route.startDFA.CallAction(UtYacc3Ns.YaccActions.CallAction);
#endif
    }

    private static void UtAction4()
    {
        string s = @"
%{        
%}
%type <string> a
%%
a: a 'A' {Console.WriteLine(""a with A"");} 
|  a 'B' {Console.WriteLine(""a with B"");} 
|  'C' {Console.WriteLine(""single C"");} 
|  'D' {Console.WriteLine(""single D"");} 
;
%%
";
        Yacc yacc = new Yacc(s);
        YaccCodeGen.GenCode(s, "UtYacc4", LexYaccUtil.GetGenFileFolder(), false);

#if !DisableGenCodeUt
        tokens.Clear();
        tokens.Add(Terminal.BuildConstCharTerminal('C'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('A'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        tokens.Add(Terminal.BuildConstCharTerminal('B'));
        yacc.Feed(tokens);

        Console.WriteLine("\n\nUtAction4 output");
        yacc.route.startDFA.CallAction(UtYacc4Ns.YaccActions.CallAction);
#endif
    }

    private static void UtAction5()
    {
        string s = @"
%{        
%}
%token <int> NUMBER
%type <int> cal exp term
%%
cal: exp {Console.WriteLine(""Result = "" + $1);} ;
exp: exp '-' NUMBER {$$ = $1 - $3;} | exp '+' NUMBER {$$ = $1 + $3;} | NUMBER {$$ = $1;};
%%
";
        /*

        cal: exp

        exp: NUMBER exp'
        exp': '-' NUMBER exp' | empty

         */

        Yacc yacc = new Yacc(s);
        YaccCodeGen.GenCode(s, "UtYacc5", LexYaccUtil.GetGenFileFolder(), false);

#if !DisableGenCodeUt
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER", 2));
        tokens.Add(Terminal.BuildConstCharTerminal('+'));
        tokens.Add(Terminal.BuildToken("NUMBER", 3));
        tokens.Add(Terminal.BuildConstCharTerminal('+'));
        tokens.Add(Terminal.BuildToken("NUMBER", 4));
        tokens.Add(Terminal.BuildConstCharTerminal('-'));
        tokens.Add(Terminal.BuildToken("NUMBER", 1));
        yacc.Feed(tokens);

        Console.WriteLine("\n\nUtAction5 output");
        yacc.route.startDFA.CallAction(UtYacc5Ns.YaccActions.CallAction);

#endif
    }

    private static void UtAction6()
    {
        string s = @"
%{        
%}
%token <int> NUMBER
%type <int> cal exp term
%%
cal: exp {Console.WriteLine(""Result = "" + $1); };
 
exp: exp '+' term {$$ = $1 + $3;}
  | term {$$ = $1;}
  ;
  
term: NUMBER {$$ = $1;}
  ;
%%
";

        Yacc yacc = new Yacc(s);
        YaccCodeGen.GenCode(s, "UtYacc6", LexYaccUtil.GetGenFileFolder(), false);

#if !DisableGenCodeUt
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER", 1));
        tokens.Add(Terminal.BuildConstCharTerminal('+'));
        tokens.Add(Terminal.BuildToken("NUMBER", 2));
        tokens.Add(Terminal.BuildConstCharTerminal('+'));
        tokens.Add(Terminal.BuildToken("NUMBER", 4));
        yacc.Feed(tokens);

        Console.WriteLine("\n\nUtAction6 output");
        yacc.route.startDFA.CallAction(UtYacc6Ns.YaccActions.CallAction);
#endif
    }

    private static void UtAction7()
    {
        string s = @"
%{        
%}
%token <int> NUMBER
%type <int> cal exp term
%%
cal: exp {$$ = $1; Console.WriteLine(""Result = "" + $1); };
 
exp:
  exp '+' term {$$ = $1 + $3;}
  | exp '-' term {$$ = $1 - $3;}
  | term {$$ = $1;}
  ;
  
term:
  term '*' NUMBER { $$ = $1 * $3;}
  | term '/' NUMBER { $$ = $1 / $3;}
  | NUMBER {$$ = $1;}
  ;
%%
";

        Yacc yacc = new Yacc(s);
        YaccCodeGen.GenCode(s, "UtYacc7", LexYaccUtil.GetGenFileFolder(), false);

#if !DisableGenCodeUt
        tokens.Clear();
        tokens.Add(Terminal.BuildToken("NUMBER", 2));
        tokens.Add(Terminal.BuildConstCharTerminal('*'));
        tokens.Add(Terminal.BuildToken("NUMBER", 3));
        tokens.Add(Terminal.BuildConstCharTerminal('-'));
        tokens.Add(Terminal.BuildToken("NUMBER", 4));
        tokens.Add(Terminal.BuildConstCharTerminal('/'));
        tokens.Add(Terminal.BuildToken("NUMBER", 2));
        tokens.Add(Terminal.BuildConstCharTerminal('+'));
        tokens.Add(Terminal.BuildToken("NUMBER", 10));
        tokens.Add(Terminal.BuildConstCharTerminal('*'));
        tokens.Add(Terminal.BuildToken("NUMBER", 100));
        yacc.Feed(tokens);

        Console.WriteLine("\n\nUtAction7 output");
        object ret = yacc.route.startDFA.CallAction(UtYacc7Ns.YaccActions.CallAction);
        Check((int)ret == 1004);

#endif
    }

    private static void UtAction()
    {
        UtAction1();
        UtAction2();
        UtAction3();
        UtAction4();
        UtAction5();
        UtAction6();
        UtAction7();
    }

    public static void RunAllUt()
    {
        //mojo
        //UtFeed25();
        //UtFeed26();

        UtSecctionSplitter();
        UtFeed();
        UtBuild();
        UtAction();
    }
}