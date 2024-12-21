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


        public static void AdhocUt()
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

            ret = sql_lexyacc.Parse("DELETE FROM A WHERE NAME = 'ABC' OR AGE = 55 ");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("SELECT * FROM A WHERE 1 = 1");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("UPDATE A SET AGE = 11, NAME = 'NNNN' WHERE 1 = 1");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A VALUES ( 'GH', 456  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("(1 + 2) * 3");
            Check(ret == null || ret.ToString() == "");

            //ret = sql_lexyacc.Parse("1 + 2 - 'adb' /  (aaa)");
            //Check(ret == null || ret.ToString() == "");

            //ret = sql_lexyacc.Parse("SELECT AGE, NAME, * FROM A WHERE 1 = 1");
            //Check(ret == null || ret.ToString() == "");
#endif
        }

        public static void Ut()
        {
            AdhocUt();
            Console.Interactive();
        }

    }
}
