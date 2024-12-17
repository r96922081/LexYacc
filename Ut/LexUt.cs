using LexYaccNs;
using System.Diagnostics;

public class LexUt
{
    public static void Check(bool b)
    {
        if (!b)
            Trace.Assert(false);
    }

    public static void GenCodeUt1()
    {
        string ruleSection = @"
%{
%}

%%
(\-)?[0-9]+ { 
        value = int.Parse(yytext);
        return NUMBER;
}
""+""  {return '+';}
""-""  {return '-';}
""*""  {return '*';}
""/""  {return '/';}
"
+ "[ \t\n]+   {}"
+ "\n%%";

        LexTokenDef l = new LexTokenDef();
        l.type = "int";
        l.name = "NUMBER";
        l.index = 256;
        List<LexTokenDef> lexTokenDef = new List<LexTokenDef>() { l };

        string namespaceStr = "UtLex1";
        LexCodeGen.GenCode(ruleSection, namespaceStr, lexTokenDef, LexYaccUtil.GetGenFileFolder(), false);
    }

    public static void GenCodeUt()
    {
        GenCodeUt1();
    }

    public static void RunLexUt1()
    {
        string input = "-0 - 2 * 3 - 12 / 4 -    \t\n  -10000     ";

#if !DisableGenCodeUt
        List<Terminal> tokens = Lex.Parse(input, UtLex1Ns.LexActions.ruleInput, UtLex1Ns.LexActions.CallAction);
        Check(tokens[0].tokenName == "NUMBER");
        Check((int)tokens[0].tokenObject == 0);
        Check(tokens[1].type == TerminalType.CONSTANT_CHAR);
        Check(tokens[1].constCharValue == "-");
        Check(tokens[10].tokenName == "NUMBER");
        Check((int)tokens[10].tokenObject == -10000);
#endif
    }

    public static void RunLexUt()
    {
        RunLexUt1();
    }

    public static void RunAllUt()
    {
        GenCodeUt();
        RunLexUt();
    }
}
