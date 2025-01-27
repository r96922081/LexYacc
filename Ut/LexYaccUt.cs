using LexYaccNs;
using MyDBNs;
using System.Diagnostics;

public class LexYaccUt
{
    public static void Check(bool b)
    {
        if (!b)
            Trace.Assert(false);
    }

    public static void RunAllUt()
    {
        // gen code
        LexYaccCodeGen.GenCode("../../../Ut/LexYaccInput/pair.l", "../../../Ut/LexYaccInput/pair.y", UtUtil.GetLexYaccOutput(), "pair");
        LexYaccCodeGen.GenCode("../../../Ut/LexYaccInput/cal.l", "../../../Ut/LexYaccInput/cal.y", UtUtil.GetLexYaccOutput(), "cal");
        LexYaccCodeGen.GenCode("../../../Ut/LexYaccInput/c_grammar.l", "../../../Ut/LexYaccInput/c_grammar.y", UtUtil.GetLexYaccOutput(), "c_grammar");
        LexYaccCodeGen.GenCode("../../../Applications/MyDBNs/LexYaccInput/sql.l", "../../../Applications/MyDBNs/LexYaccInput/sql_statements.y", "../../../Applications/MyDBNs/LexYaccOutput/", "sql_statements");
        LexYaccCodeGen.GenCode("../../../Applications/MyDBNs/LexYaccInput/sql.l", "../../../Applications/MyDBNs/LexYaccInput/sql_boolean_expression.y", "../../../Applications/MyDBNs/LexYaccOutput/", "sql_boolean_expression");
        LexYaccCodeGen.GenCode("../../../Applications/MyDBNs/LexYaccInput/sql.l", "../../../Applications/MyDBNs/LexYaccInput/sql_arithmetic_expression.y", "../../../Applications/MyDBNs/LexYaccOutput/", "sql_arithmetic_expression");
        LexYaccCodeGen.GenCode("../../../Applications/CCompilerNs/LexYaccInput/cc.l", "../../../Applications/CCompilerNs/LexYaccInput/cc.y", "../../../Applications/CCompilerNs/LexYaccOutput", "cc");

#if !DisableGenCodeUt
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
}
