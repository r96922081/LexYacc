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
            object ret = sql_arithmetic_expression.Parse("3 - 2 + 1");
            Check((double)ret == 2);

            ret = sql_arithmetic_expression.Parse("12 / 6 / 2");
            Check((double)ret == 1);

            ret = sql_arithmetic_expression.Parse("9 / 3 * 4 ");
            Check((double)ret == 12);

            ret = sql_arithmetic_expression.Parse("(1 + 2) * 3 / ( (8 - 2) / 2) * 4 ");
            Check((double)ret == 12);

            ret = sql_lexyacc.Parse("SELECT * FROM A WHERE AGE != 6 - 3 - 2");
            Check(ret == null || ret.ToString() == "");
#endif
        }

        public static void TestBooleanExpression()
        {
#if !MarkUserOfSqlCodeGen
            object ret = sql_lexyacc.Parse("DELETE FROM A WHERE NAME = 'ABC'  ");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("SELECT * FROM A WHERE NAME != 'ABC'");
            Check(ret == null || ret.ToString() == "");
#endif
        }

        public static void TestSql()
        {
#if !MarkUserOfSqlCodeGen

            object ret = sql_lexyacc.Parse("CREATE TABLE A ( NAME VARCHAR(123), AGE NUMBER)");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("CREATE TABLE A2 ( AAA VARCHAR(456), BBB NUMBER)");
            Check(ret == null || ret.ToString() == "");


            ret = sql_lexyacc.Parse("INSERT INTO A VALUES ( 'DEF', 33  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A ( NAME, AGE ) VALUES ( 'DEF', 33  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A VALUES ( 44, 55  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A ( AGE, NAME ) VALUES ( 66, 'ABC'  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A ( AGE ) VALUES ( 999)");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("SHOW TABLES");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A VALUES ( 'GH', 456  )");
            Check(ret == null || ret.ToString() == "");
#endif
        }

        public static void Ut()
        {
            TestSql();
            TestBooleanExpression();
            TestArithmeticExpression();
            Console.Interactive();
        }

    }
}
