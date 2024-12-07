using LexYaccNs;
using System.Diagnostics;

public class LexYaccUt
{
    public static void Check(bool b)
    {
        if (!b)
            Trace.Assert(false);
    }

    public static void LexYaccUt1()
    {
        // gen code
        LexYaccCodeGen.GenCode("../../../input/pair.l", "../../../input/pair.y", LexYaccUtil.GetGenFileFolder(), "pair");
        LexYaccCodeGen.GenCode("../../../input/cal.l", "../../../input/cal.y", LexYaccUtil.GetGenFileFolder(), "cal");
        LexYaccCodeGen.GenCode("../../../input/c_grammar.l", "../../../input/c_grammar.y", LexYaccUtil.GetGenFileFolder(), "c_grammar");
        LexYaccCodeGen.GenCode("../../../input/sql.l", "../../../input/sql.y", LexYaccUtil.GetGenFileFolder(), "sql_lexyacc");
        LexYaccCodeGen.GenCode("../../../input/sql.l", "../../../input/sql_condition.y", LexYaccUtil.GetGenFileFolder(), "sql_condition_lexyacc");

#if GenCodeUt
        object ret = cal.Parse(" 2 * 3 + 6 / 2 + 10000  ");
        Check((int)ret == 10009);

        ret = pair.Parse("     key1 = 11 key2 =2222  key3 = 888");

        string input = @"
void main(int a, int b) 
{
    int c = 2 + 3;
    return c * a - 5;
}
";
        ret = c_grammar.Parse(input);
#endif
    }


    public static void RunAllUt()
    {
        LexYaccUt1();
    }
}
