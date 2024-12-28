using System.Diagnostics;

namespace MyDBNs
{
    public class SqlUt
    {
        public static void Check(bool b)
        {
            if (!b)
                Trace.Assert(false);
        }


        public static void TestArithmeticExpression()
        {
#if !MarkUserOfSqlCodeGen
            object ret = sql_statements.Parse("UPDATE A SET AGE = AGE + 2 * 100");
            Check(ret == null || ret.ToString() == "");

            ret = sql_statements.Parse("UPDATE A SET NAME = NAME || '_X'");
            Check(ret == null || ret.ToString() == "");

            ret = sql_statements.Parse("SELECT * FROM A WHERE NAME = 'ABC' || '_X'");
            Check(ret == null || ret.ToString() == "");

            /*
            object ret = sql_arithmetic_expression.Parse("3 - 2 + 1");
            Check((double)ret == 2);

            ret = sql_arithmetic_expression.Parse("12 / 6 / 2");
            Check((double)ret == 1);

            ret = sql_arithmetic_expression.Parse("9 / 3 * 4 ");
            Check((double)ret == 12);

            ret = sql_arithmetic_expression.Parse("(1 + 2) * 3 / ( (8 - 2) / 2) * 4 ");
            Check((double)ret == 12);*/

            //ret = sql_statements.Parse("SELECT * FROM A WHERE AGE != 6 - 3 - 2");
            //Check(ret == null || ret.ToString() == "");
#endif
        }

        public static void Ut()
        {
            Gv.ut = true;
            new CreateDropUt().Ut();
            new InsertUt().Ut();
            new SelectUt().Ut();
            new BooleanExpressionUt().Ut();
            new SaveLoadUt().Ut();

            TestArithmeticExpression();
            Console.Interactive();
        }

    }
}
